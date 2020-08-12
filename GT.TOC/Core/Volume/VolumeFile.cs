using GT.Shared.Logging;

namespace GT.TOC.Core.Volume
{
    public abstract class VolumeFile
    {
        protected readonly string _basePath;
        protected readonly ILogWriter _logger;
        protected bool _isVolume;
        protected IVolumeHeader _volumeHeader;
        protected IVolumeSegment _volumeSegment;

        protected VolumeFile(string basePath, ILogWriter logger = null)
        {
            _basePath = basePath;
            _logger = logger;
        }

        public abstract (string Seed, uint[] Key) KeySet { get; }

        public void CryptHeader(ref byte[] data, int size, bool encrypt = false, bool needSwapEndian = true)
        {
            var key = new uint[4];
            MainCrypt.DataKeygen(KeySet.Seed, KeySet.Key, 1, key);
            if (!encrypt)
            {
                MainCrypt.DataCrypt(key, data, data, size);
                MainCrypt.BlockCrypt(data, out data, size, needSwapEndian: needSwapEndian);
                return;
            }

            MainCrypt.BlockCrypt(data, out data, size, true, needSwapEndian); // TODO: Check this works
            MainCrypt.DataCrypt(key, data, data, size);
        }

        public byte[] CryptSegment(byte[] data, uint size, uint index)
        {
            var key = new uint[4];
            MainCrypt.DataKeygen(KeySet.Seed, KeySet.Key, index, key);
            MainCrypt.DataCrypt(key, data, data, size);
            return data;
        }

        public void InflateDataIfNeeded(ref byte[] buffer, ulong outSize)
        {
            buffer = PS2Zip.Inflate(buffer);
        }

        public bool Load()
        {
            if (!ReadHeader(out var headerData))
                return false;

            if (!DecryptHeader(ref headerData))
                return false;

            if (!ParseHeader(headerData))
                return false;

            if (!ParseSegment())
                return false;

            return true;
        }

        protected abstract int GetHeaderSize();
        protected abstract bool NeedSwapEndian();
        protected abstract bool ReadHeader(out byte[] headerData);
        protected abstract bool DecryptHeader(ref byte[] headerData);
        protected abstract bool ParseHeader(byte[] headerData);
        protected abstract bool ParseSegment();
        public abstract PackedFile GetPackedFile();
    }
}
