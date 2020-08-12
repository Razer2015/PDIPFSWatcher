using System;
using System.IO;

namespace GT.TOC.Core
{
    public class FileIDBTree
    {
        public const byte kDIRECTORY_FLAG = (1 << 0); // 1
        public const byte kFILE_FLAG = (1 << 1); // 2
        public const byte kFILE_WITHOUT_EXTENSION_FLAG = (1 >> 1); // 0

        public byte Flag { get; set; }

        public uint NameIndex { get; set; }

        public uint ExtensionIndex { get; set; }

        public uint EntryIndex { get; set; }

        public FileIDBTree() { }

        public FileIDBTree(EndianBinReader reader, ref uint offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            Flag = reader.ReadByte();
            offset++;
            NameIndex = Util.ExtractValueAndAdvance(reader, ref offset);
            ExtensionIndex = (Flag & kFILE_FLAG) != 0 ? Util.ExtractValueAndAdvance(reader, ref offset) : 0;
            EntryIndex = Util.ExtractValueAndAdvance(reader, ref offset);
        }

        public static uint SkipNodeData(EndianBinReader node, uint ptr)
        {
            uint unk1 = Util.ExtractValueAndAdvance(node, ref ptr);
            return (ptr);
        }

        public static FileIDBTree Parse(EndianBinReader reader, uint offset)
        {
            FileIDBTree FileID = new FileIDBTree(reader, ref offset);
            return (FileID);
        }

        public override string ToString()
        {
            return
                $"Flag: {Flag} - NameIndex: {NameIndex} - ExtensionIndex: {ExtensionIndex} - EntryIndex: {EntryIndex}";
        }

        public string ToString(StringBTree[] names, StringBTree[] extensions)
        {
            return
                $"Flag: {Flag} - Name: {names[NameIndex]} ({NameIndex}) - Extension: {extensions[ExtensionIndex]} ({ExtensionIndex}) - EntryIndex: {EntryIndex}";
        }
    }
}
