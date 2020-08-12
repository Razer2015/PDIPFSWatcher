using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIPFSWatcher
{
    using System;
    using System.IO;
    using System.Security.Permissions;

    public class Watcher
    {
        public event EventHandler FileAccessed;

        private readonly string _folder;
        private FileSystemWatcher _fileWatcher;

        public Watcher(string folder)
        {
            _folder = folder;
            _fileWatcher = new FileSystemWatcher();

            Initialize();
        }

        public void StartWatching()
        {
            // Begin watching.
            _fileWatcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            // Stop watching.
            _fileWatcher.EnableRaisingEvents = false;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void Initialize()
        {
            _fileWatcher.Path = _folder;

            // Watch for changes in LastAccess
            _fileWatcher.NotifyFilter = NotifyFilters.LastAccess;

            // Watch all files and sub files
            _fileWatcher.Filter = "";
            _fileWatcher.IncludeSubdirectories = true;

            // Add event handlers.
            _fileWatcher.Changed += OnChanged;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e) =>
            // Specify what is done when a file is changed, created, or deleted.
            FileAccessed?.Invoke(source, e);
    }
}
