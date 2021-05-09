// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single class

using System;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Contains event data about file events notified by a <see cref="DirectoryWatcher"/>.
    /// </summary>
    public class FileEvent : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="FileEvent"/> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="name">The name of the file.</param>
        /// <param name="fullPath">The full path of the file.</param>
        public FileEvent(FileEventChangeType changeType, string name, string fullPath)
        {
            ChangeType = changeType;
            Name = name;
            FullPath = fullPath;
        }

        /// <summary>
        ///   Gets the type of the change.
        /// </summary>
        /// <value>The type of the change.</value>
        public FileEventChangeType ChangeType { get; }

        /// <summary>
        ///   Gets the name of the file.
        /// </summary>
        /// <value>The file name.</value>
        public string Name { get; }

        /// <summary>
        ///   Gets the full path of the file.
        /// </summary>
        /// <value>The full path of the file.</value>
        public string FullPath { get; }
    }

    /// <summary>
    ///   Contains event data about file renaming events notified by a <see cref="DirectoryWatcher"/>.
    /// </summary>
    public class FileRenameEvent : FileEvent
    {
        /// <summary>
        ///   Gets the full path (in case of rename).
        /// </summary>
        /// <value>The full path (in case of rename).</value>
        public string OldFullPath { get; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="FileRenameEvent"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fullPath">The full path of the file.</param>
        /// <param name="oldFullPath">The old full path (before rename).</param>
        public FileRenameEvent(string name, string fullPath, string oldFullPath)
            : base(FileEventChangeType.Renamed, name, fullPath)
        {
            OldFullPath = oldFullPath;
        }


        public override string ToString()
        {
            return string.Format("{0}: {1} -> {2}", ChangeType, FullPath, OldFullPath);
        }
    }
}
