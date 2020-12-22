// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Represents an abstract base class for <see cref="IVirtualFileProvider"/>.
    /// </summary>
    public abstract class VirtualFileProviderBase : IVirtualFileProvider
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="VirtualFileProviderBase"/> class.
        /// </summary>
        /// <param name="rootPath">The root path of this provider. That is the path all other paths will be relative to.</param>
        protected VirtualFileProviderBase(string rootPath)
        {
            RootPath = rootPath;

            // Ensure RootPath ends with /
            if (RootPath != null)
            {
                if (string.IsNullOrWhiteSpace(RootPath))
                    throw new ArgumentException(nameof(rootPath));

                if (!RootPath.EndsWith(VirtualFileSystem.DirectorySeparatorChar))
                    RootPath += VirtualFileSystem.DirectorySeparatorChar;
            }
            VirtualFileSystem.RegisterProvider(this);
        }

        /// <inheritdoc/>
        public string RootPath { get; private set; }

        /// <inheritdoc/>
        public virtual string GetAbsolutePath(string path) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual bool TryGetFileLocation(string path, out string filePath, out long start, out long end)
        {
            filePath = null;
            start = 0;
            end = -1;

            return false;
        }

        /// <inheritdoc/>
        public abstract Stream OpenStream(string url, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share = VirtualFileShare.Read, StreamFlags streamFlags = StreamFlags.None);

        /// <summary>
        ///   Resolves the specified path, mapping virtual to absolute paths.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns>The resolved path.</returns>
        protected virtual string ResolvePath(string path) => path;

        /// <inheritdoc/>
        public virtual bool DirectoryExists(string url) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual string[] ListFiles(string url, string searchPattern, VirtualSearchOption searchOption) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual bool FileExists(string url) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual void FileDelete(string url) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual void FileMove(string sourceUrl, string destinationUrl) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual void FileMove(string sourceUrl, IVirtualFileProvider destinationProvider, string destinationUrl) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual long FileSize(string url) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual DateTime GetLastWriteTime(string url) => throw new NotImplementedException();

        /// <inheritdoc/>
        public virtual void CreateDirectory(string url) => throw new NotImplementedException();

        public void Dispose()
        {
            VirtualFileSystem.UnregisterProvider(this);
        }
    }
}
