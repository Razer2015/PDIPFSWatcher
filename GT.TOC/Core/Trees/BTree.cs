using System.Collections.Generic;
using System.IO;
using System.Linq;
using GT.Shared.Logging;
using GT.Shared.Threading;

namespace GT.TOC.Core
{
    public class BTree
    {
        private readonly ILogWriter _logWriter;
        private readonly PackedFile _packedFile;

        public BTree(PackedFile packedFile, ILogWriter logWriter = null)
        {
            _packedFile = packedFile;
            _logWriter = logWriter;
        }

        #region Traversing

        public List<(string Path, uint EntryIndex, string PDPath)> GetAllDisplayFiles()
        {
            uint index = 0;
            uint depth;

            var files = new List<(string Path, uint EntryIndex, string PDPath)>();
            // Root Nodes
            for (int i = 0; i < _packedFile.FileIDs[0].Length; i++)
            {
                if ((_packedFile.FileIDs[0][i].Flag == FileIDBTree.kDIRECTORY_FLAG)) // If = directory
                {
                    string baseDir = _packedFile.Names[_packedFile.FileIDs[0][i].NameIndex].Text +
                                     Path.AltDirectorySeparatorChar;
                    depth = _packedFile.FileIDs[0][i].EntryIndex;
                    TraverseDisplaySubTree(files, baseDir, ref depth, ref index);
                }
                else if ((_packedFile.FileIDs[0][i].Flag == FileIDBTree.kFILE_FLAG) ||
                         (_packedFile.FileIDs[0][i].Flag == FileIDBTree.kFILE_WITHOUT_EXTENSION_FLAG)) // If = file
                {
                    var entryIndex = _packedFile.FileIDs[0][i].EntryIndex;
                    files.Add((_packedFile.Names[_packedFile.FileIDs[0][i].NameIndex].Text, entryIndex,
                        PDIPFSPath.GenerateFilePath(entryIndex)));

                    index++;
                }
            }

            return files;
        }

        private void TraverseDisplaySubTree(List<(string Path, uint EntryIndex, string PDPath)> files, string path,
            ref uint depth, ref uint index)
        {
            string baseDir = path;
            uint baseDepth = depth;
            for (int i = 0; i < _packedFile.FileIDs[depth].Length; i++)
            {
                switch (_packedFile.FileIDs[depth][i].Flag)
                {
                    case FileIDBTree.kDIRECTORY_FLAG:
                        path += _packedFile.Names[_packedFile.FileIDs[depth][i].NameIndex].Text +
                                Path.AltDirectorySeparatorChar;
                        depth = _packedFile.FileIDs[depth][i].EntryIndex;
                        TraverseDisplaySubTree(files, path, ref depth, ref index);
                        path = baseDir;
                        depth = baseDepth;
                        break;

                    case FileIDBTree.kFILE_FLAG:
                    case FileIDBTree.kFILE_WITHOUT_EXTENSION_FLAG:
                        string newPath = path + _packedFile.Names[_packedFile.FileIDs[depth][i].NameIndex].Text;
                        if (_packedFile.FileIDs[depth][i].ExtensionIndex != 0
                        ) // AKA: fileid_btree.kFILE_WITHOUT_EXTENSION_FLAG
                        {
                            newPath += _packedFile.Extensions[_packedFile.FileIDs[depth][i].ExtensionIndex].Text;
                        }

                        var entryIndex = _packedFile.FileIDs[depth][i].EntryIndex;
                        files.Add((newPath, entryIndex, PDIPFSPath.GenerateFilePath(entryIndex)));
                        index++;
                        break;
                }
            }
        }

        private List<(string Path, uint EntryIndex)> GetAllFiles()
        {
            uint index = 0;
            uint depth;

            var files = new List<(string Path, uint EntryIndex)>();
            // Root Nodes
            for (int i = 0; i < _packedFile.FileIDs[0].Length; i++)
            {
                if ((_packedFile.FileIDs[0][i].Flag == FileIDBTree.kDIRECTORY_FLAG)) // If = directory
                {
                    string baseDir = _packedFile.Names[_packedFile.FileIDs[0][i].NameIndex].Text +
                                     Path.AltDirectorySeparatorChar;
                    depth = _packedFile.FileIDs[0][i].EntryIndex;
                    TraverseSubTree(files, baseDir, ref depth, ref index);
                }
                else if ((_packedFile.FileIDs[0][i].Flag == FileIDBTree.kFILE_FLAG) ||
                         (_packedFile.FileIDs[0][i].Flag == FileIDBTree.kFILE_WITHOUT_EXTENSION_FLAG)) // If = file
                {
                    files.Add((_packedFile.Names[_packedFile.FileIDs[0][i].NameIndex].Text,
                        _packedFile.FileIDs[0][i].EntryIndex));

                    index++;
                }
            }

            return files;
        }

        private void TraverseSubTree(List<(string Path, uint EntryIndex)> files, string path, ref uint depth,
            ref uint index)
        {
            string baseDir = path;
            uint baseDepth = depth;
            for (int i = 0; i < _packedFile.FileIDs[depth].Length; i++)
            {
                switch (_packedFile.FileIDs[depth][i].Flag)
                {
                    case FileIDBTree.kDIRECTORY_FLAG:
                        path += _packedFile.Names[_packedFile.FileIDs[depth][i].NameIndex].Text +
                                Path.AltDirectorySeparatorChar;
                        depth = _packedFile.FileIDs[depth][i].EntryIndex;
                        TraverseSubTree(files, path, ref depth, ref index);
                        path = baseDir;
                        depth = baseDepth;
                        break;

                    case FileIDBTree.kFILE_FLAG:
                    case FileIDBTree.kFILE_WITHOUT_EXTENSION_FLAG:
                        string newPath = path + _packedFile.Names[_packedFile.FileIDs[depth][i].NameIndex].Text;
                        if (_packedFile.FileIDs[depth][i].ExtensionIndex != 0
                        ) // AKA: fileid_btree.kFILE_WITHOUT_EXTENSION_FLAG
                        {
                            newPath += _packedFile.Extensions[_packedFile.FileIDs[depth][i].ExtensionIndex].Text;
                        }

                        files.Add((newPath, _packedFile.FileIDs[depth][i].EntryIndex));

                        index++;
                        break;
                }
            }
        }

        #endregion
    }
}
