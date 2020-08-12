using System;
using System.Linq;
using System.IO;

namespace GT.TOC.Core
{
    public class MainCrypt
    {
        static uint Checksum(string data, long size)
        {
            uint result = ~Hash.CRC32_0x04c11db7(0, data, size);
            return result;
        }

        static uint XorShiftLoop(uint x, uint y)
        {
            uint result = x;
            for (int i = 0; i < 32; ++i)
            {
                uint upper_bit = result & 0x80000000u;
                result <<= 1;
                if (Convert.ToBoolean(upper_bit))
                    result ^= y;
            }

            return result;
        }

        static uint InversedXorShiftLoop(uint x, uint y)
        {
            uint result = ~XorShiftLoop(x, y);
            return result;
        }

        static uint ShuffleBits(uint data)
        {
            uint x = data;
            uint crc = 0;
            for (int i = 0; i < 4; ++i)
            {
                crc = (crc << 8) ^ Hash.kCRC32_TAB_0x04C11DB7[(Util.RotateLeft(x ^ crc, 10) & 0x3FC) >> 2];
                x <<= 8;
            }

            return ~crc;
        }

        static uint CryptBlock(uint x, uint y)
        {
            uint result = x ^ ShuffleBits(y);
            return result;
        }

        public static void DataKeygen(string seed, uint[] key, uint x, uint[] out_key)
        {
            uint c0 = Checksum(seed, seed.Length) ^ x;
            uint c1 = InversedXorShiftLoop(c0, key[0]);
            uint c2 = InversedXorShiftLoop(c1, key[1]);
            uint c3 = InversedXorShiftLoop(c2, key[2]);
            uint c4 = InversedXorShiftLoop(c3, key[3]);

            c1 &= 0x1FFFFu;
            c2 &= 0x7FFFFu;
            c3 &= 0x7FFFFFu;
            c4 &= 0x1FFFFFFFu;

            out_key[0] = c1;
            out_key[1] = c2;
            out_key[2] = c3;
            out_key[3] = c4;
        }

        public static void DataCrypt(uint[] key, byte[] src, byte[] dst, long size)
        {
            uint c1 = key[0];
            uint c2 = key[1];
            uint c3 = key[2];
            uint c4 = key[3];

            byte[] src_byte = src;
            byte[] dst_byte = dst;

            int index = 0;
            while (index < size)
            {
                dst_byte[index] = (byte)((((c1 ^ c2) ^ src_byte[index]) ^ (c3 ^ c4)) & 0xFF);
                c1 = ((Util.RotateLeft(c1, 9) & 0x1FE00u) | (c1 >> 8));
                c2 = ((Util.RotateLeft(c2, 11) & 0x7F800u) | (c2 >> 8));
                c3 = ((Util.RotateLeft(c3, 15) & 0x7F8000u) | (c3 >> 8));
                c4 = ((Util.RotateLeft(c4, 21) & 0x1FE00000u) | (c4 >> 8));
                index++;
            }
        }

        public static bool BlockCrypt(byte[] src, out byte[] dst, long size, bool encrypt = false,
            bool needSwapEndian = true)
        {
            byte[] src_block = src;
            byte[] buffer = new byte[4];
            MemoryStream ms = new MemoryStream(src_block);
            MemoryStream ms2 = new MemoryStream();

            ms.Read(buffer, 0, 4);
            int prev = BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);
            ms2.Write(buffer, 0, 4);

            int index = 1;
            while (index < (size / 4))
            {
                ms.Read(buffer, 0, 4);
                int cur = BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);
                uint cryptoResult = CryptBlock((uint)cur, (uint)prev);

                var cryptResult = BitConverter.GetBytes(cryptoResult);
                if (needSwapEndian) Array.Reverse(cryptResult);
                ms2.Write(cryptResult.ToArray(), 0, 4);
                if (encrypt)
                    prev = (int)cryptoResult;
                else
                    prev = cur;
                index++;
            }

            dst = ms2.ToArray();
            ms.Close();
            ms2.Close();
            return true;
        }
    }
}
