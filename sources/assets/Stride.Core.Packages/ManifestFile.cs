// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Packages
{
    /// <summary>
    /// Describe a file in a package by giving the <see cref="Source"/> of a file or set of files, the destination <see cref="Target"/> where they will be copied
    /// with some exclude rules <see cref="Exclude"/>.
    /// Both Source and Exclude can use regular expressions.
    /// </summary>
    public class ManifestFile
    {
        /// <summary>
        /// Set of source files that will be copied to <see cref="Target"/>.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Target location where files described by <see cref="Source"/> will be copied.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Rules excluding copies of files from <see cref="Source"/>.
        /// </summary>
        public string Exclude { get; set; }
    }
}
