// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

using Stride.Core.Diagnostics;

namespace Stride.Core.IO
{
    public partial class DirectoryWatcher
    {
        private readonly Dictionary<string, DirectoryWatcherItem> watchers = new Dictionary<string, DirectoryWatcherItem>(StringComparer.CurrentCultureIgnoreCase);

        private void InitializeInternal()
        {
            watcherCheckThread = new Thread(SafeAction.Wrap(RunCheckWatcher)) { IsBackground = true, Name = "Directory Watcher thread" };
            watcherCheckThread.Start();
        }

        private void DisposeInternal()
        {
            foreach (var watcher in watchers.Values)
            {
                if (watcher.Watcher != null)
                {
                    DisposeNativeWatcher(watcher.Watcher);
                }
                watcher.Watcher = null;
            }
            watchers.Clear();
        }

        private List<string> GetTrackedDirectoriesInternal()
        {
            List<string> directories;
            lock (watchers)
            {
                directories = ListTrackedDirectories().Select(pair => pair.Key).ToList();
            }
            directories.Sort();
            return directories;
        }

        private void TrackInternal(string path)
        {
            var info = GetDirectoryInfoFromPath(path);
            if (info is null)
                return;

            lock (watchers)
            {
                Track(info, true);
            }
        }

        private void UnTrackInternal(string path)
        {
            var info = GetDirectoryInfoFromPath(path);
            if (info is null)
                return;

            lock (watchers)
            {
                if (!watchers.TryGetValue(info.FullName, out DirectoryWatcherItem watcher))
                    return;

                UnTrack(watcher, true);
            }
        }

        private void RunCheckWatcher()
        {
            try
            {
                while (!exitThread)
                {
                    // TODO: Should use a wait on an event in order to cancel it more quickly instead of a blocking Thread.Sleep
                    Thread.Sleep(SleepBetweenWatcherCheck);

                    lock (watchers)
                    {
                        var list = ListTrackedDirectories().ToList();
                        foreach (var watcherKeyPath in list)
                        {
                            if (!watcherKeyPath.Value.PathExists)
                            {
                                UnTrack(watcherKeyPath.Value, removeWatcherFromGlobals: true);
                                OnModified(
                                    sender: this,
                                    new FileEvent(FileEventChangeType.Deleted, Path.GetFileName(watcherKeyPath.Value.Path), watcherKeyPath.Value.Path));
                            }
                        }

                        // If no more directories are tracked, clear completely the watchers tree
                        if (!ListTrackedDirectories().Any())
                        {
                            watchers.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unexpected end of DirectoryWatcher thread. {ex}");
            }
        }

        private IEnumerable<KeyValuePair<string, DirectoryWatcherItem>> ListTrackedDirectories()
        {
            return watchers.Where(pair => pair.Value.Watcher != null);
        }

        private DirectoryInfo GetDirectoryInfoFromPath(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));

            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));

            // Extract directory information from path
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            if (path != null && Directory.Exists(path))
                return new DirectoryInfo(path.ToLowerInvariant());
            else
                return null;
        }

        private IEnumerable<DirectoryWatcherItem> ListTracked(IEnumerable<DirectoryInfo> directories)
        {
            foreach (var directoryInfo in directories)
            {
                if (watchers.TryGetValue(directoryInfo.FullName, out DirectoryWatcherItem watcher))
                {
                    yield return watcher;
                }
            }
        }

        private IEnumerable<DirectoryWatcherItem> ListTrackedChildren(DirectoryWatcherItem watcher)
        {
            return ListTracked(watcher.ListChildrenDirectories());
        }

        private int CountTracked(IEnumerable<DirectoryInfo> directories)
        {
            return ListTracked(directories).Count(watcher => watcher.Watcher != null);
        }

        private DirectoryWatcherItem Track(DirectoryInfo info, bool watcherNode)
        {
            if (watchers.TryGetValue(info.FullName, out DirectoryWatcherItem watcher))
            {
                if (watcher.Watcher is null && watcherNode)
                {
                    watcher.Watcher = CreateFileSystemWatcher(watcher.Path);
                }
                watcher.TrackCount++;

                return watcher;
            }

            var parent = info.Parent != null ? Track(info.Parent, false) : null;

            if (parent != null && watcherNode)
            {
                if (parent.Watcher != null)
                {
                    return parent;
                }

                var childrenDirectoryList = parent.ListChildrenDirectories().ToList();
                var countTracked = CountTracked(childrenDirectoryList);

                var newCount = countTracked + 1;
                if (newCount == childrenDirectoryList.Count && newCount > 1)
                {
                    UnTrack(parent, false);
                    parent.Watcher = CreateFileSystemWatcher(parent.Path);
                    return parent;
                }
            }

            watcher = new DirectoryWatcherItem(info) { Parent = parent };
            if (watcherNode)
            {
                watcher.Watcher = CreateFileSystemWatcher(watcher.Path);
            }
            watchers.Add(watcher.Path, watcher);

            watcher.TrackCount++;
            return watcher;
        }

        private void UnTrack(DirectoryWatcherItem watcher, bool removeWatcherFromGlobals)
        {
            foreach (var child in ListTrackedChildren(watcher))
            {
                UnTrack(child, true);
            }

            watcher.TrackCount--;

            if (watcher.TrackCount == 0)
            {
                if (watcher.Watcher != null)
                {
                    DisposeNativeWatcher(watcher.Watcher);
                    watcher.Watcher = null;
                }

                watcher.Parent = null;

                if (removeWatcherFromGlobals)
                {
                    watchers.Remove(watcher.Path);
                }
            }
        }

        private void DisposeNativeWatcher(FileSystemWatcher watcher)
        {
            //Console.WriteLine("Watcher disposing {0}", watcher.Path);
            watcher.EnableRaisingEvents = false;
            watcher.Changed -= OnModified;
            watcher.Created -= OnModified;
            watcher.Deleted -= OnModified;
            watcher.Renamed -= OnModified;
            watcher.Error -= WatcherOnError;
            watcher.Dispose();
            //Console.WriteLine("Watcher disposed {0}", watcher.Path);
        }

        protected FileSystemWatcher CreateFileSystemWatcher(string directory)
        {
            //Console.WriteLine("Watcher creating {0}", directory);
            var watcher = new FileSystemWatcher()
                {
                    Path = directory,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = FileFilter,
                    IncludeSubdirectories = true,
                };

            watcher.Changed += OnModified;
            watcher.Created += OnModified;
            watcher.Deleted += OnModified;
            watcher.Renamed += OnModified;
            watcher.Error += WatcherOnError;

            watcher.EnableRaisingEvents = true;

            //Console.WriteLine("Watcher created {0}", directory);
            return watcher;
        }

        private void WatcherOnError(object sender, ErrorEventArgs errorEventArgs)
        {
            try
            {
                //Console.WriteLine("DirectoryWatcher faile Watcher exception: {0}", errorEventArgs.GetException());
                lock (watchers)
                {
                    var watcher = watchers.Values.FirstOrDefault(item => item.Watcher == sender);
                    if (watcher != null)
                    {
                        // Remove a specific watcher if there was any error with it
                        UnTrack(watcher, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Unexpected exception in WatcherOnError: {0}", ex));
            }
        }

        private void OnModified(object sender, FileSystemEventArgs e)
        {
            lock (watchers)
            {
                if (e.ChangeType == WatcherChangeTypes.Deleted && watchers.TryGetValue(e.FullPath, out DirectoryWatcherItem watcher))
                {
                    UnTrack(watcher, true);
                }
            }

            var handler = Modified;
            if (handler != null)
            {
                if (e.ChangeType == WatcherChangeTypes.Renamed)
                {
                    var renamedEventArgs = e as RenamedEventArgs;
                    OnModified(this, new FileRenameEvent(e.Name, e.FullPath, renamedEventArgs.OldFullPath));
                }
                else
                {
                    OnModified(this, new FileEvent((FileEventChangeType)e.ChangeType, e.Name, e.FullPath));
                }
            }
        }

        [DebuggerDisplay("Active: {IsActive}, Path: {Path}")]
        private sealed class DirectoryWatcherItem
        {
            public DirectoryWatcherItem Parent;
            public string Path { get; private set; }
            public bool PathExists => Directory.Exists(Path);
            public int TrackCount { get; set; }
            public FileSystemWatcher Watcher { get; set; }
            private bool IsActive => Watcher != null;


            public DirectoryWatcherItem(DirectoryInfo path)
            {
                Path = path.FullName.ToLowerInvariant();
            }


            public IEnumerable<DirectoryInfo> ListChildrenDirectories()
            {
                var info = new DirectoryInfo(Path);
                try
                {
                    if (info.Exists)
                    {
                        return info.EnumerateDirectories();
                    }
                }
                catch (Exception)
                {
                    // An exception can occur if the file is being removed
                }

                return Enumerable.Empty<DirectoryInfo>();
            }
        }
    }
}
