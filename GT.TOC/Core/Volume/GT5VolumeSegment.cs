using GT.Shared;
using Ionic.Zlib;

namespace GT.TOC.Core.Volume
{
    public struct GT5VolumeSegment : IVolumeSegment
    {
        public uint Magic;
        public uint Size { get; set; }
        public uint RealSize { get; set; }

        public uint DataSize { get; set; }
        public byte[] Data { get; set; }

        public bool Read(EndianBinReader reader, uint size, uint realSize)
        {
            Size = size;
            RealSize = realSize;

            if ((Magic = reader.ReadUInt32()) != 0xC5EEF7FFu)
                return false;

            DataSize = reader.ReadUInt32();
            DataSize = (uint)-DataSize;

            Size -= Consts.kVOLUME_SEGMENT_HEADER_SIZE;

            Data = reader.ReadBytes((int)Size);
            Data = DeflateStream.UncompressBuffer(Data);
            System.Diagnostics.Debug.Assert(Data.Length == RealSize);

            return true;
        }
    }
}
