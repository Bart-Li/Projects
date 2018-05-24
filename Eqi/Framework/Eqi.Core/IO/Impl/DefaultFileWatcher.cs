using System;
using System.IO;

namespace Eqi.Core.IO.Impl
{
    [DependencyInjection(typeof(IFileWatcher), LifeTime = ServiceLifeTime.Transient)]
    public class DefaultFileWatcher : IFileWatcher
    {
        private FileSystemWatcher _watcher;
        private Action _action;

        /// <summary>
        /// Watch data change.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="action">Callback event.</param>
        public void WatchDataChange(string filePath, Action action)
        {
            this.WatchDataChange(filePath, string.Empty, action);
        }

        /// <summary>
        /// Watch data change.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="filter">File filter.</param>
        /// <param name="action">Callback event.</param>
        public void WatchDataChange(string filePath, string filter, Action action)
        {
            if (!Directory.Exists(filePath))
            {
                throw new ArgumentException($"file path:[{filePath}] not exists.");
            }

            this._action = action;
            if (string.IsNullOrEmpty(filter))
            {
                this._watcher = new FileSystemWatcher(filePath);
            }
            else
            {
                this._watcher = new FileSystemWatcher(filePath, filter);
            }

            this._watcher.Changed += new FileSystemEventHandler(Watcher_Process);
            this._watcher.Deleted += new FileSystemEventHandler(Watcher_Process);
            this._watcher.IncludeSubdirectories = true;
            this._watcher.EnableRaisingEvents = true;
            this._watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.FileName;
        }

        private void Watcher_Process(object sender, FileSystemEventArgs e)
        {
            this._action?.Invoke();
        }
    }
}
