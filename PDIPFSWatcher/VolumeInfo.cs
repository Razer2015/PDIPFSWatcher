using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.Shared;
using GT.Shared.Logging;
using GT.TOC.Core;
using GT.TOC.Core.Volume;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using PDIPFSWatcher.Models;
using PDIPFSWatcher.Tracing;

namespace PDIPFSWatcher
{
    public class VolumeInfo
    {
        public event EventHandler NewFileAccess;
        public readonly TraceManager TraceManager = new TraceManager();

        private readonly string _folder;

        //private Watcher _watcher;
        private BTree _bTree;
        private List<(string path, uint entryIndex, string pdPath)> _allFiles;

        public VolumeInfo(string folder)
        {
            _folder = folder;

            Initialize();
        }

        public void StartWatching()
        {
            // Begin watching.
            //_watcher.StartWatching();

            TraceManager.Start(new List<EventType> {EventType.FileRead});
        }

        public void StopWatching()
        {
            // Stop watching.
            //_watcher.StopWatching();

            TraceManager.Stop();
        }

        private void Initialize()
        {
            var volumes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(s => s.ManifestModule.ScopeName.Contains("GT.TOC"))
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(VolumeFile).IsAssignableFrom(p) && !p.IsAbstract);

            VolumeFile volumeFile = null;
            foreach (var vol in volumes)
            {
                volumeFile = (VolumeFile)Activator.CreateInstance(vol, _folder, new ConsoleWriter());
                if (volumeFile.Load())
                    break;

                volumeFile = null;
            }

            if (volumeFile == null)
                throw new Exception("Unable to load volume file.");

            _bTree = new BTree(volumeFile.GetPackedFile());
            _allFiles = _bTree.GetAllDisplayFiles();

            //_watcher = new Watcher(_folder);
            //_watcher.FileAccessed += FileAccessed;


            TraceManager.EventTrace += (evt, type) =>
            {
                var fileInfo = evt as FileIOReadWriteTraceData;

                if (!fileInfo.FileName.StartsWith(_folder)) return;

                var file = _allFiles.FirstOrDefault(x =>
                    x.pdPath == $"{fileInfo.FileName.Replace(_folder, string.Empty)}");
                NewFileAccess?.Invoke(null,
                    new PDIPFSFileAccessEventArgs
                    {
                        PDIPFS = file.pdPath,
                        HRPath = file.path,
                        NodeIndex = file.entryIndex,
                        Offset = fileInfo.Offset,
                        Size = fileInfo.IoSize,
                        IrpPtr = fileInfo.IrpPtr
                    });
            };
        }

        private void FileAccessed(object sender, EventArgs e)
        {
            var fileName = (FileSystemEventArgs)e;

            var file = _allFiles.FirstOrDefault(x => x.pdPath == $"\\{fileName.Name}");
            NewFileAccess?.Invoke(sender,
                new PDIPFSFileAccessEventArgs {PDIPFS = file.pdPath, HRPath = file.path, NodeIndex = file.entryIndex});
        }
    }
}
