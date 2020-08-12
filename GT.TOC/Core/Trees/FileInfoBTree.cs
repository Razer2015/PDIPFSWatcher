using System.IO;

namespace GT.TOC.Core
{
    public class FileInfoBTree
    {
        public const uint kFLAG = (1 << 0);

        public byte CompressedFlag { get; set; }
        public uint EntryIndex { get; set; }
        public uint CompressedSize { get; set; }
        public uint UncompressedSize { get; set; }
        public uint SegmentIndex { get; set; }
        public FileInfoBTree() { }

        public FileInfoBTree(EndianBinReader reader, ref uint offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            CompressedFlag = reader.ReadByte();
            offset++;
            EntryIndex = Util.ExtractValueAndAdvance(reader, ref offset);
            CompressedSize = Util.ExtractValueAndAdvance(reader, ref offset);
            UncompressedSize = (CompressedFlag & kFLAG) != 0
                ? Util.ExtractValueAndAdvance(reader, ref offset)
                : CompressedSize;
            SegmentIndex = Util.ExtractValueAndAdvance(reader, ref offset);
        }

        public static uint SkipNodeData(EndianBinReader node, uint ptr)
        {
            return (0);
        }

        public static FileInfoBTree Parse(EndianBinReader reader, uint offset)
        {
            FileInfoBTree FileInfo = new FileInfoBTree(reader, ref offset);
            return (FileInfo);
        }

        public override string ToString()
        {
            return
                $"Compressed: {CompressedFlag} - EntryIndex: {EntryIndex} - CompressedSize: {CompressedSize} - UncompressedSize: {UncompressedSize} - SegmentIndex: {SegmentIndex}";
        }
    }
}
