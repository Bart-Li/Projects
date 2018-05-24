using System;

namespace Eqi.Core.IO
{
    /// <summary>
    /// File watcher.
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Watch data change.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="action">Callback event.</param>
        void WatchDataChange(string filePath, Action action);

        /// <summary>
        /// Watch data change.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="filter">File filter.</param>
        /// <param name="action">Callback event.</param>
        void WatchDataChange(string filePath, string filter, Action action);
    }
}
