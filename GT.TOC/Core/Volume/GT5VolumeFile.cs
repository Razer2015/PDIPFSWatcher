using System.IO;
using GT.Shared;
using GT.Shared.Logging;

namespace GT.TOC.Core.Volume
{
    public class GT5VolumeFile : VolumeFile
    {
        public GT5VolumeFile(string basePath, ILogWriter logger = null) : base(basePath, logger) { }

        public override (string Seed, uint[] Key) KeySet => ("KALAHARI-37863889",
            new uint[] {0x2DEE26A7, 0x412D99F5, 0x883C94E9, 0x0F1A7069});

        protected override int GetHeaderSize() { return 0xA0; }
        protected override bool NeedSwapEndian() { return true; }

        protected override bool ReadHeader(out byte[] headerData)
        {
            headerData = null;

            var attr = File.GetAttributes(_basePath);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var headerPath = Path.Combine($"{_basePath.TrimEnd('\\')}\\",
                    PDIPFSPath.GenerateFilePath(1).TrimStart('\\'));

                if (!File.Exists(headerPath)) return false;

                headerData = File.ReadAllBytes(headerPath);

                return true;
            }

            _isVolume = true;
            using (var fs = new FileStream(_basePath, FileMode.Open, FileAccess.Read))
            {
                headerData = new byte[GetHeaderSize()];
                fs.Read(headerData, 0, GetHeaderSize());

                return true;
            }
        }

        protected override bool DecryptHeader(ref byte[] headerData)
        {
            if (headerData == null || headerData.Length <= 0) return false;

            CryptHeader(ref headerData, GetHeaderSize(), needSwapEndian: NeedSwapEndian());
#if DEBUG
            File.WriteAllBytes("decryptedHeader.bin", headerData);
#endif

            return true;
        }

        protected override bool ParseHeader(byte[] headerData)
        {
            using (var ms = new MemoryStream(headerData))
            using (var reader = new EndianBinReader(ms))
            {
                _volumeHeader = new GT5VolumeHeader();
                return _volumeHeader.Read(reader);
            }
        }

        protected override bool ParseSegment()
        {
            byte[] segmentData;
            if (_isVolume)
            {
                using (var fs = new FileStream(_basePath, FileMode.Open, FileAccess.Read))
                {
                    segmentData = new byte[_volumeHeader.Size];
                    fs.Read(segmentData, (int)Consts.kVOLUME_SEGMENT_SIZE, segmentData.Length);
                }
            }
            else
            {
                var segmentPath = Path.Combine($"{_basePath.TrimEnd('\\')}\\",
                    PDIPFSPath.GenerateFilePath(_volumeHeader.Seed).TrimStart('\\'));
                if (!File.Exists(segmentPath)) return false;

                segmentData = File.ReadAllBytes(segmentPath);
            }

            CryptSegment(segmentData, _volumeHeader.Size, _volumeHeader.Seed);

#if DEBUG
            File.WriteAllBytes("compressedSegment.bin", segmentData);
#endif

            using (var ms = new MemoryStream(segmentData))
            using (var reader = new EndianBinReader(ms))
            {
                _volumeSegment = new GT5VolumeSegment();
                if (!_volumeSegment.Read(reader, _volumeHeader.Size, _volumeHeader.RealSize)) return false;
            }

#if DEBUG
            File.WriteAllBytes("segmentData.bin", _volumeSegment.Data);
#endif
            return true;
        }

        public override PackedFile GetPackedFile()
        {
            using (var ms = new MemoryStream(_volumeSegment.Data))
            using (var reader = new EndianBinReader(ms))
            {
                var packedFile = new PackedFile();
                packedFile.Load(reader, true);
                return packedFile;
            }
        }
    }
}
