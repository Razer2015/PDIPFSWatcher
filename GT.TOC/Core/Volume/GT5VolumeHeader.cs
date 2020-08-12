using System.Text;
using GT.Shared;

namespace GT.TOC.Core.Volume
{
    public struct GT5VolumeHeader : IVolumeHeader
    {
        public uint Magic;
        public uint Seed { get; set; }
        public uint Size { get; set; }
        public uint RealSize { get; set; }
        public ulong PatchSequence;
        public ulong FileSize;
        public string TitleID;


        public bool Read(EndianBinReader reader)
        {
            if ((Magic = reader.ReadUInt32()) != Consts.kVOLUME_HEADER_MAGIC)
                return false;

            Seed = reader.ReadUInt32();
            if (Seed <= 0) return false;

            Size = reader.ReadUInt32();
            RealSize = reader.ReadUInt32();
            PatchSequence = reader.ReadUInt64();
            FileSize = reader.ReadUInt64();
            var buffer = reader.ReadBytes(128);
            TitleID = Encoding.UTF8.GetString(buffer);

            return true;
        }
    }
}
