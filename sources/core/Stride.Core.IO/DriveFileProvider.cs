// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Text;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Represents a <see cref="FileSystemProvider"/> that exposes the whole file system through a folder
    ///   with paths in the form <c>"/c/Program Files/Test/Data.dat"</c>.
    /// </summary>
    public class DriveFileProvider : FileSystemProvider
    {
        public static string DefaultRootPath = "/drive";


        public DriveFileProvider(string rootPath) : base(rootPath, null) { }


        /// <summary>
        ///   Resolves the virtual path from a given file path.
        /// </summary>
        /// <param name="filePath">The file path to resolve.</param>
        /// <returns>The virtual local path of the corresponding file.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string GetLocalPath(string filePath)
        {
            filePath = Path.GetFullPath(filePath);

            var resolveProviderResult = VirtualFileSystem.ResolveProvider(RootPath, resolveTop: true);
            if (!(resolveProviderResult.Provider is DriveFileProvider provider))
                throw new InvalidOperationException();

            return provider.ConvertFullPathToUrl(filePath);
        }

        /// <inheritdoc/>
        protected override string ConvertUrlToFullPath(string url)
        {
            // Linux style: keep as is
            if (VolumeSeparatorChar == '/')
                return url;

            // TODO: Support more complex URL such as UNC or devices
            // Windows style: reprocess URL like Cygwin
            var result = new StringBuilder(url.Length + 1);
            int separatorIndex = 0;

            foreach (char ch in url.ToCharArray())
            {
                if (ch == VirtualFileSystem.DirectorySeparatorChar ||
                    ch == VirtualFileSystem.AltDirectorySeparatorChar)
                {
                    if (separatorIndex == 1)
                    {
                        // Add volume separator on second /
                        result.Append(VolumeSeparatorChar);
                    }

                    // Ignore first separator (before volume)
                    if (separatorIndex >= 1)
                    {
                        result.Append(DirectorySeparatorChar);
                    }

                    separatorIndex++;
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }

        /// <inheritdoc/>
        protected override string ConvertFullPathToUrl(string path)
        {
            // Linux style: keep as is
            if (VolumeSeparatorChar == '/')
                return path;

            // TODO: Support more complex URL such as UNC or devices
            // Windows style: reprocess URL like Cygwin
            var result = new StringBuilder(path.Length + 1);

            result.Append(VirtualFileSystem.DirectorySeparatorChar);

            foreach (char ch in path.ToCharArray())
            {
                if (ch == VolumeSeparatorChar)
                {
                    // TODO: More advanced validation, i.e. is there no directory separator before volume separator, etc...
                }
                else if (ch == DirectorySeparatorChar)
                {
                    result.Append(VirtualFileSystem.DirectorySeparatorChar);
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }
    }
}
