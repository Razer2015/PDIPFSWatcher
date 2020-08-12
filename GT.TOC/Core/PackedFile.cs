using System.IO;

namespace GT.TOC.Core
{
    public partial class PackedFile
    {
        public const uint kMAGIC = 0x5B74516Eu;
        public const long kHEADER_SIZE = 0x14;
        public const ulong kSEGMENT_SIZE = 0x800;

        private uint _nameTableOffset;
        private uint _extensionTableOffset;
        private uint _fileInfoTableOffset;
        private uint _numFileIdTrees;
        private uint[] _fileIdOffsets;

        public uint SegmentSize { get; set; }
        public uint FileCount { get; set; }

        public StringBTree[] Names { get; set; }
        public StringBTree[] Extensions { get; set; }
        public FileInfoBTree[] FileInfos { get; set; }
        public FileIDBTree[][] FileIDs { get; set; }


        public PackedFile()
        {
            SegmentSize = 0;
            _nameTableOffset = 0;
            _extensionTableOffset = 0;
            _fileInfoTableOffset = 0;
            _numFileIdTrees = 0;
            _fileIdOffsets = null;
            FileCount = 0;
        }

        public void Load(byte[] data, uint dataSize, uint segmentSize)
        {
            using (var ms = new MemoryStream(data))
            using (var reader = new EndianBinReader(ms))
            {
                Load(reader, dataSize, segmentSize);
            }
        }

        public void Load(EndianBinReader reader, uint dataSize, uint segmentSize)
        {
            if (reader.ReadUInt32() != kMAGIC) return;

            SegmentSize = segmentSize;

            _nameTableOffset = reader.ReadUInt32();
            _extensionTableOffset = reader.ReadUInt32();
            _fileInfoTableOffset = reader.ReadUInt32();
            _numFileIdTrees = reader.ReadUInt32();

            Load(reader);
        }
    }
}
