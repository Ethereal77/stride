// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Core.Packages
{
    /// <summary>
    /// Representation of a dependency in a package manifest.
    /// </summary>
    public class ManifestDependency
    {
        /// <summary>
        /// Name of package dependency.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Version of package dependency.
        /// </summary>
        public PackageVersionRange Version { get; set; }
    }
}
