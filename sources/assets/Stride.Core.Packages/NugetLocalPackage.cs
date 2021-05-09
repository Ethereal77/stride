// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using NuGet.Protocol;

namespace Stride.Core.Packages
{
    /// <summary>
    /// Description of a package that has been installed locally.
    /// </summary>
    public class NugetLocalPackage : NugetPackage
    {
        /// <summary>
        /// A new instance of <see cref="NugetLocalPackage"/> initialized from <see cref="LocalPackageSearchMetadata"/>.
        /// </summary>
        /// <param name="info">The NuGet local information about the package.</param>
        internal NugetLocalPackage(LocalPackageInfo info) : base(new LocalPackageSearchMetadata(info))
        {
            Info = info;
        }

        /// <summary>
        /// The copyright of the current local package.
        /// </summary>
        public string Copyright => Info.Nuspec.GetCopyright();

        /// <summary>
        /// The release notes of the current local package.
        /// </summary>
        public string ReleaseNotes => Info.Nuspec.GetReleaseNotes();

        /// <summary>
        /// The language of the current local package.
        /// </summary>
        public string Language => Info.Nuspec.GetLanguage();

        /// <summary>
        /// Nupkg path.
        /// </summary>
        public string NupkgPath => Info.IsNupkg ? Info.Path : null;

        /// <summary>
        /// Folder containing nupkg and extracted package.
        /// </summary>
        public string Path => Info.IsNupkg ? Directory.GetParent(Info.Path).FullName : Info.Path;

        /// <summary>
        /// Gets the list of files that make up the current local package.
        /// </summary>
        /// <returns>The list of files making up the current local package.</returns>
        public IEnumerable<PackageFile> GetFiles()
        {
            var res = new List<PackageFile>();
            var files = Info.GetReader().GetFiles();
            if (files != null)
            {
                foreach (var file in files)
                {
                    res.Add(new PackageFile(Path, file));
                }
            }
            return res;
        }

        /// <summary>
        /// The reader of the associated .nuspec file of the current local package.
        /// </summary>
        protected LocalPackageInfo Info { get; }
    }
}
