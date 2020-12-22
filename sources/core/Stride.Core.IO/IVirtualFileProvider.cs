// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Defines the interface of a virtual file provider, that can return a <see cref="Stream"/> for a given path.
    /// </summary>
    public interface IVirtualFileProvider : IDisposable
    {
        /// <summary>
        ///   Gets or sets the root path of this provider.
        /// </summary>
        /// <remarks>
        ///   All paths are relative to the root path.
        /// </remarks>
        string RootPath { get; }

        /// <summary>
        ///   Gets the absolute path for the specified local path from this provider.
        /// </summary>
        /// <param name="path">The path local to this instance.</param>
        /// <returns>An absolute path.</returns>
        string GetAbsolutePath(string path);

        /// <summary>
        ///   Gets the absolute path and location if the specified path physically exists on the disk in an
        ///   uncompressed form (could be inside another file).
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filePath">The file containing the data.</param>
        /// <param name="start">The start offset in the file.</param>
        /// <param name="end">The end offset in the file. This can be -1 if is the whole file.</param>
        /// <returns>
        ///   <c>true</c> if successful; <c>false</c> if not supported and entry is found.
        ///   (NOTE: Even when true, the file might not actually exist).
        /// </returns>
        bool TryGetFileLocation(string path, out string filePath, out long start, out long end);

        /// <summary>
        ///   Opens a stream from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode in which the operating system will open the file..</param>
        /// <param name="access">The file access mode..</param>
        /// <param name="share">Specifies how other processes share the access to this same file. Default is <see cref="VirtualFileShare.Read"/>.</param>
        /// <param name="streamFlags">The type of stream needed. Default is <see cref="StreamFlags.None"/>.</param>
        /// <returns>The opened <see cref="Stream"/>.</returns>
        Stream OpenStream(string path, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share = VirtualFileShare.Read, StreamFlags streamFlags = StreamFlags.None);

        /// <summary>
        ///   Returns a list of files from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <param name="searchOption">The search option.</param>
        /// <returns>A list of files from the specified path.</returns>
        string[] ListFiles(string path, string searchPattern, VirtualSearchOption searchOption);

        /// <summary>
        ///   Creates the specified directory. Also creates all directories needed for the specified
        ///   path to exist.
        /// </summary>
        /// <param name="url">The path to create.</param>
        void CreateDirectory(string url);

        /// <summary>
        ///   Determines whether the specified path points to an existing directory.
        /// </summary>
        /// <param name="url">The path.</param>
        /// <returns><c>true</c> if the directory exists; <c>false</c> otherwise.</returns>
        bool DirectoryExists(string url);

        /// <summary>
        ///   Determines whether the specified path points to an existing file.
        /// </summary>
        /// <param name="url">The path.</param>
        /// <returns><c>true</c> if the file exists; <c>false</c> otherwise.</returns>
        bool FileExists(string url);

        /// <summary>
        ///   Deletes the specified file.
        /// </summary>
        /// <param name="url">The path.</param>
        void FileDelete(string url);

        /// <summary>
        ///   Moves the specified file from one location to another.
        /// </summary>
        /// <param name="sourceUrl">The source path of the file to move.</param>
        /// <param name="destinationUrl">The destination path of the file.</param>
        /// <remarks>
        ///   The file will not be overwriten. An exception will be thrown if the file can't be moved.
        /// </remarks>
        /// <exception cref="IOException">The file couldn't be moved.</exception>
        void FileMove(string sourceUrl, string destinationUrl);

        /// <summary>
        ///   Moves the specified file from one location to another.
        /// </summary>
        /// <param name="sourceUrl">The source path of the file to move.</param>
        /// <param name="destinationProvider">The destination provider.</param>
        /// <param name="destinationUrl">The destination path of the file, relative to the destination provider.</param>
        /// <remarks>
        ///   The file will not be overwriten. An exception will be thrown if the file can't be moved.
        /// </remarks>
        /// <exception cref="IOException">The file couldn't be moved.</exception>
        void FileMove(string sourceUrl, IVirtualFileProvider destinationProvider, string destinationUrl);

        /// <summary>
        ///   Returns the size of the specified file, in bytes
        /// </summary>
        /// <param name="url">The file or directory for which to obtain size.</param>
        /// <returns>The file size in bytes.</returns>
        long FileSize(string url);

        /// <summary>
        ///   Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="url">The file or directory for which to obtain write date and time information.</param>
        /// <returns>
        ///   A <see cref="DateTime"/> set to the date and time that the specified file or directory was last written to.
        /// </returns>
        DateTime GetLastWriteTime(string url);
    }
}
