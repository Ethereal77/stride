// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Nerdbank.GitVersioning
{
    /// <summary>
    ///   Reads the version from a Stride Package (<c>.sdpkg</c>). Implemented for <see cref="GitExtensions"/>.
    /// </summary>
    internal class VersionFile
    {
        /// <summary>
        ///   Reads the version from a given <c>.sdpkg</c> file.
        /// </summary>
        /// <param name="packagePath">Stride Package file path.</param>
        /// <returns><see cref="VersionOptions"/> with the Stride Package version.</returns>
        public static VersionOptions GetVersion(string packagePath)
        {
            try
            {
                using (var fileStream = File.OpenRead(packagePath))
                {
                    return GetVersionFromStream(fileStream);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///   Reads the version from a given <c>.sdpkg</c> file in a specific Git commit.
        /// </summary>
        /// <param name="commit"><see cref="LibGit2Sharp.Commit"/> in which to look for <paramref cref="packagePath"/>.</param>
        /// <param name="packagePath">Stride Package file path.</param>
        /// <returns><see cref="VersionOptions"/> with the Stride Package version.</returns>
        public static VersionOptions GetVersion(LibGit2Sharp.Commit commit, string packagePath)
        {
            if (commit is null)
                return null;

            try
            {
                if (commit.Tree[packagePath]?.Target is LibGit2Sharp.Blob packageData)
                    return GetVersionFromStream(packageData.GetContentStream());
            }
            catch { }

            return null;
        }

        private static VersionOptions GetVersionFromStream(Stream stream)
        {
            // Load the asset as a YamlNode object
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                var text = reader.ReadToEnd();

                var publicVersion = Regex.Match(text, "PublicVersion = \"(.*)\";");
                if (!publicVersion.Success || !Version.TryParse(publicVersion.Groups[0].Value, out var parsedVersion))
                    return null;

                return new VersionOptions { Version = parsedVersion };
            }
        }
    }
}
