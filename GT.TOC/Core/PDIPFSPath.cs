using System;
using System.Collections.Generic;

namespace GT.TOC.Core
{
    /// <summary>
    ///     Converted to C# by Razerman and is based on the gttool source by flatz
    /// </summary>
    public class PDIPFSPath
    {
        public PDIPFSPath(uint fileIndice)
        {
            GenerateFilePath(fileIndice);
        }

        public PDIPFSPath(string pdipfsPath)
        {
            GenerateKeyFromPath(pdipfsPath);
        }

        public PDIPFSPath(List<string> pdipfsPathList)
        {
            GenerateKeyFromPathList(pdipfsPathList);
        }

        public static uint GenerateIndexFromPath(string pdipfsPath)
        {
            return GenerateKeyFromPath(pdipfsPath);
        }

        static string _descriptor;
        static uint _file_indice;

        //static List<string> _descriptor_list;
        static List<uint> _file_indice_list;

        private static uint GenerateKeyFromPath(string path)
        {
            bool found = false;
            uint index = 0;

            // K 5 9 W 4
            if (path[1] == 'K') { index = 0; }

            if (path[1] == '5') { index = 1024; }

            if (path[1] == '9') { index = 32768; }

            if (path[1] == 'W') { index = 1048576; }

            if (path[1] == '4') { index = 33554432; }

            while (!found)
            {
                if (GenerateFilePath(index) == path)
                {
                    found = true;
                }
                else
                {
                    index++;
                }
            }

            _file_indice = index;
            return index;
        }

        private static void GenerateKeyFromPathList(List<string> paths)
        {
            _file_indice_list = new List<uint>();
            foreach (string path in paths)
            {
                _file_indice_list.Add(GenerateKeyFromPath(path));
            }
        }

        private static uint GenerateFilePathKey(uint c, uint keyLength, uint id)
        {
            if (keyLength == 0)
                return id;

            uint mask = 1u << ((int)keyLength);
            uint key = id;
            for (uint i = 0; i < keyLength; ++i)
            {
                key <<= 1;
                if ((key & mask) != 0)
                    key ^= c;
            }

            return key;
        }

        static void StringizeFilePathKey(uint key, uint keyLength)
        {
            if (keyLength == 0)
            {
            }
            else
            {
                char[] ALPHABET =
                {
                    'K', '5', '9', 'W', '4', 'S', '6', 'H', '7', 'D', 'O', 'V', 'J', 'P', 'E', 'R', 'U', 'Q', 'M',
                    'T', '8', 'B', 'A', 'I', 'C', '2', 'Y', 'L', 'G', '3', '0', 'Z', '1', 'F', 'N', 'X'
                };

                char[] keyString = new char[16];

                ulong x = key;
                for (uint i = 0; i < keyLength; ++i)
                {
                    ulong y = ((x * 954437177) >> 32) >> 3;
                    ulong z = x - (y * 4 + y * 32);
                    System.Diagnostics.Debug.Assert(z < (ulong)ALPHABET.Length);
                    keyString[i] = ALPHABET[z];
                    x = y;
                }

                uint pos;
                if (keyLength % 2 == 0)
                {
                    _descriptor += '\\';
                    _descriptor += keyString[keyLength - 1];
                    pos = keyLength - 2;
                }
                else
                {
                    pos = keyLength - 1;
                }

                while (true)
                {
                    _descriptor += keyString[pos];
                    if (pos == 0)
                        break;
                    _descriptor += '\\';
                    _descriptor += keyString[pos - 1];
                    pos -= 2;
                }
            }
        }

        public static string GenerateFilePath(uint id)
        {
            _descriptor = "\\";
            uint x = id;
            if (x < 1024)
            {
                _descriptor += 'K';
                StringizeFilePathKey(GenerateFilePathKey(1177, 10, x), 2);
                return (_descriptor);
            }

            x -= 1024;
            if (x < 32768)
            {
                _descriptor += '5';
                StringizeFilePathKey(GenerateFilePathKey(34961, 15, x), 3);
                return (_descriptor);
            }

            x -= 32768;
            if (x < 1048576)
            {
                _descriptor += '9';
                StringizeFilePathKey(GenerateFilePathKey(1120393, 20, x), 4);
                return (_descriptor);
            }

            if (x - 1048576 < 33554432)
            {
                _descriptor += 'W';
                StringizeFilePathKey(GenerateFilePathKey(35922449, 25, x - 1048576), 5);
                return (_descriptor);
            }

            if (x - 34603008 >= 0)
            {
                _descriptor += '4';
                StringizeFilePathKey(GenerateFilePathKey(2290684177, 31, x - 34603008), 6);
                return (_descriptor);
            }

            return (_descriptor);
        }
    }
}
