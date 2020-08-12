using System;
using System.IO;
using System.Text;

namespace GT.TOC.Core
{
    public class StringBTree
    {
        public uint Length { get; set; }
        public string Text { get; set; }

        public StringBTree()
        {
        }

        public void Write(EndianBinWriter writer)
        {
            writer.Write(Text);
        }

        public StringBTree(EndianBinReader reader, uint offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            Length = reader.ReadByte();
            Text = Encoding.ASCII.GetString(reader.ReadBytes((int)Length));
        }

        public StringBTree(byte[] data, ref uint offset)
        {
            Length = data[offset];
            Text = Encoding.ASCII.GetString(data, (int)(offset + 1), (int)Length);
        }

        public static uint SkipNodeData(EndianBinReader node, uint ptr)
        {
            uint length = Util.ExtractValueAndAdvance(node, ref ptr);
            return (length);
        }

        public static string Parse(EndianBinReader node, uint ptr)
        {
            uint length = Util.ExtractValueAndAdvance(node, ref ptr);
            string data = Encoding.ASCII.GetString(node.ReadBytes((int)length));
            return (data);
        }

        public struct Key
        {
            public EndianBinReader data;
            public uint length;

            public Key(EndianBinReader _data, uint _length)
            {
                data = _data;
                length = _length;
            }
        };

        public static int KeyEqualOP(Key key, EndianBinReader data)
        {
            EndianBinReader ptr = data;

            uint length = Util.ExtractValueAndAdvance(ptr);
            uint min_length = Math.Min(key.length, length);

            EndianBinReader p1 = (key.data);
            EndianBinReader p2 = (ptr);

            for (uint i = 0; i < min_length; ++i)
            {
                byte _p1 = p1.ReadByte();
                byte _p2 = p2.ReadByte();

                if (_p1 < _p2)
                    return -1;
                else if (_p1 > _p2)
                    return 1;
            }

            if (key.length < length)
                return -1;
            else if (key.length > length)
                return 1;
            else
                return 0;
        }

        public static int KeyLessThanOP(Key key, EndianBinReader data)
        {
            EndianBinReader ptr = data;
            uint index = Util.ExtractValueAndAdvance(ptr);
            int result = KeyEqualOP(key, ptr);
            return result != 0 ? result : 1;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
