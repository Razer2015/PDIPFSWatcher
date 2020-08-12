using System;

namespace GT.TOC.Core
{
    public partial class PackedFile
    {
        public bool Load(EndianBinReader reader, bool readHeader = false)
        {
            if (readHeader)
            {
                if (reader.ReadUInt32() != kMAGIC)
                {
                    return (false);
                }

                _nameTableOffset = reader.ReadUInt32();
                _extensionTableOffset = reader.ReadUInt32();
                _fileInfoTableOffset = reader.ReadUInt32();
                _numFileIdTrees = reader.ReadUInt32();
            }

            _fileIdOffsets = new uint[_numFileIdTrees];
            for (int i = 0; i < _numFileIdTrees; i++)
            {
                _fileIdOffsets[i] = reader.ReadUInt32();
            }

            // Read Names
            Names = LoadStringBTree(reader, _nameTableOffset);
            // Read Extensions
            Extensions = LoadStringBTree(reader, _extensionTableOffset);
            // Read FileInfo
            FileInfos = LoadFileInfoBTree(reader, _fileInfoTableOffset);

            // Read FileID:s
            int index = 0;
            FileIDs = new FileIDBTree[_fileIdOffsets.Length][];

            foreach (uint offset in _fileIdOffsets)
            {
                FileIDBTree[] temp = new FileIDBTree[0];
                LoadFileIDBTree(reader, ref temp, offset);

                FileIDs[index] = temp;
                index++;
            }

            // Get the file count
            for (int i = 0; i < FileIDs.Length; i++)
            for (int j = 0; j < FileIDs[i].Length; j++)
                if (FileIDs[i][j].Flag == FileIDBTree.kFILE_FLAG ||
                    FileIDs[i][j].Flag == FileIDBTree.kFILE_WITHOUT_EXTENSION_FLAG)
                    FileCount++;

            return (true);
        }

        private StringBTree[] LoadStringBTree(EndianBinReader reader, uint offset)
        {
            reader.BaseStream.Seek(offset, System.IO.SeekOrigin.Begin);
            byte childCount = reader.ReadByte();
            uint childOffset = reader.Read3BytesUInt32() + offset;
            uint nodeCount = reader.ReadUInt16();
            uint dataOffset = (uint)reader.BaseStream.Position;
            var objects = new StringBTree[0];
            uint indexer = 0;
            for (uint i = 0; i < nodeCount; i++)
            {
                uint keyCount = (ushort)(Util.ExtractTwelveBits(reader, dataOffset, 0) & 0x7FF);
                uint nextNodeDataOffset =
                    (ushort)(Util.ExtractTwelveBits(reader, dataOffset, (ushort)(keyCount + 1)));
                Array.Resize(ref objects, (int)(objects.Length + keyCount));
                for (uint j = 0; j < keyCount; j++)
                {
                    uint node_offset = Util.ExtractTwelveBits(reader, dataOffset, (uint)j + 1);
                    reader.BaseStream.Position = dataOffset;
                    objects[indexer] = new StringBTree(reader, (dataOffset + node_offset));
                    indexer++;
                }

                dataOffset += nextNodeDataOffset;
            }

            return objects;
        }

        private FileInfoBTree[] LoadFileInfoBTree(EndianBinReader reader, uint offset)
        {
            reader.BaseStream.Seek(offset, System.IO.SeekOrigin.Begin);
            byte childCount = reader.ReadByte();
            uint childOffset = reader.Read3BytesUInt32();
            uint nodeCount = reader.ReadUInt16();
            uint dataOffset = offset + 4 + 2;
            var objects = new FileInfoBTree[0];
            uint indexer = 0;
            for (int i = 0; i < nodeCount; i++)
            {
                uint keyCount = (ushort)(Util.ExtractTwelveBits(reader, dataOffset, 0) & 0x7FF);
                uint nextNodeDataOffset =
                    (ushort)(Util.ExtractTwelveBits(reader, dataOffset, (ushort)(keyCount + 1)));
                Array.Resize(ref objects, (int)(objects.Length + keyCount));
                for (int j = 0; j < keyCount; j++)
                {
                    uint nodeOffset = Util.ExtractTwelveBits(reader, dataOffset, (uint)j + 1);
                    var tmpOffset = (dataOffset + nodeOffset);
                    objects[indexer] = new FileInfoBTree(reader, ref tmpOffset);
                    indexer++;
                }

                dataOffset += nextNodeDataOffset;
            }

            return objects;
        }

        private void LoadFileIDBTree(EndianBinReader reader, ref FileIDBTree[] objects, uint offset)
        {
            uint childOffset = 0;
            uint nodeCount = 0;
            uint ptrNodeData = 0;

            reader.BaseStream.Seek(offset, System.IO.SeekOrigin.Begin);
            byte childCount = reader.ReadByte();
            childOffset = reader.Read3BytesUInt32() + offset;
            nodeCount = reader.ReadUInt16();
            ptrNodeData = (uint)reader.BaseStream.Position;

            if (objects == null)
                objects = new FileIDBTree[0];
            uint indexer = (uint)objects.Length;
            for (int i = 0; i < nodeCount; i++)
            {
                uint keyCount = (ushort)(Util.ExtractTwelveBits(reader, ptrNodeData, 0) & 0x7FF);
                uint nextNodeDataOffset =
                    (ushort)(Util.ExtractTwelveBits(reader, ptrNodeData, (ushort)(keyCount + 1)));
                Array.Resize(ref objects, (int)(objects.Length + keyCount));
                for (int j = 0; j < keyCount; j++)
                {
                    uint nodeOffset = Util.ExtractTwelveBits(reader, ptrNodeData, (uint)j + 1);
                    var tmpOffset = (ptrNodeData + nodeOffset);
                    objects[indexer] = new FileIDBTree(reader, ref tmpOffset);
                    indexer++;
                }

                ptrNodeData += nextNodeDataOffset;
            }
        }
    }
}
