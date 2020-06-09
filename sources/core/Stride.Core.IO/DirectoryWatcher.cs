// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Watches for changes in the file system from one or several directories and generates events whenever something changes.
    /// </summary>
    public partial class DirectoryWatcher : IDisposable
    {
        private const int SleepBetweenWatcherCheck = 200;

        private Thread watcherCheckThread;
        private bool exitThread;

        /// <summary>
        ///   Gets the filter to use to look for changes in files.
        /// </summary>
        /// <value>The file filter.</value>
        public string FileFilter { get; private set; }


        /// <summary>
        ///   Occurs when a file / directory change occurred.
        /// </summary>
        public event EventHandler<FileEvent> Modified;


        /// <summary>
        ///   Initializes a new instance of the <see cref="DirectoryWatcher"/> class.
        /// </summary>
        /// <param name="fileFilter">The file filter of the files to watch for. If <c>null</c>, defaults to <c>*.*</c>.</param>
        public DirectoryWatcher(string fileFilter = null)
        {
            FileFilter = fileFilter ?? "*.*";

            InitializeInternal();
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            exitThread = true;
            if (watcherCheckThread != null)
            {
                watcherCheckThread.Join();
                watcherCheckThread = null;
            }

            DisposeInternal();
        }


        /// <summary>
        ///   Gets a list of the currently tracked directories.
        /// </summary>
        /// <returns>A list of the directories being tracked.</returns>
        public List<string> GetTrackedDirectories()
        {
            return GetTrackedDirectoriesInternal();
        }

        /// <summary>
        ///   Starts to look for changes in the specified path.
        /// </summary>
        /// <param name="path">The path to track. If it points to a file, this will track the parent directory.</param>
        /// <remarks>
        ///   If <paramref name="path"/> is an invalid path, it will not fail but just skip it.
        /// </remarks>
        public void Track(string path)
        {
            TrackInternal(path);
        }

        /// <summary>
        ///   Stops tracking the changes in the specified path.
        /// </summary>
        /// <param name="path">The path to un-track. If it points to a file, this will track the parent directory.</param>
        /// <remarks>
        ///  If <paramref name="path"/> is an invalid path, it will not fail but just skip it.
        /// </remarks>
        public void UnTrack(string path)
        {
            UnTrackInternal(path);
        }

        /// <summary>
        ///   Called when a file event occurred.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The file event.</param>
        protected virtual void OnModified(object sender, FileEvent e)
        {
            Modified?.Invoke(sender, e);
        }
    }
}
