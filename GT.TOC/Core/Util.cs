using System;
using System.IO;
using System.Linq;

namespace GT.TOC.Core
{
    public static class Util
    {
        public static uint RotateLeft(uint x, int n)
        {
            uint result = (x << n) | (x >> (32 - n));
            return result;
        }

        public static byte[] String2ByteArray(string word)
        {
            char[] arr = word.ToCharArray(0, word.Length);

            return arr.Select(Convert.ToByte).ToArray();
        }

        public static uint ExtractValueAndAdvance(EndianBinReader reader)
        {
            uint value = reader.ReadByte();
            if ((value & 0x80) != 0)
            {
                uint mask = 0x80;
                do
                {
                    value = ((value - mask) << 8) + reader.ReadByte();
                    mask = mask << 7;
                } while ((value & mask) != 0);
            }

            return value;
        }

        public static uint ExtractValueAndAdvance(EndianBinReader reader, ref uint ptr)
        {
            uint p = 0;
            reader.BaseStream.Seek((ptr + p++), SeekOrigin.Begin);
            uint value = reader.ReadByte();
            if ((value & 0x80) != 0)
            {
                uint mask = 0x80;
                do
                {
                    reader.BaseStream.Seek((ptr + p++), SeekOrigin.Begin);
                    value = ((value - mask) << 8) + reader.ReadByte();
                    mask = mask << 7;
                } while ((value & mask) != 0);
            }

            ptr += p;
            return value;
        }

        public static ushort ExtractTwelveBits(EndianBinReader reader, uint ptr_data, uint offset)
        {
            reader.BaseStream.Seek((int)(ptr_data + (offset * 16 - offset * 4) / 8), SeekOrigin.Begin);
            ushort result = reader.ReadUInt16();
            if ((offset & 0x1) == 0)
                result /= 16;
            return (ushort)(result & 0xFFF);
        }
    }
}
