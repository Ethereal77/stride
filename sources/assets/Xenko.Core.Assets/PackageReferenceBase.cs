// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core;

namespace Xenko.Core.Assets
{
    /// <summary>
    /// Common class used by both <see cref="PackageReference"/> and <see cref="PackageDependency"/>.
    /// </summary>
    [DataContract("PackageReferenceBase")]
    public abstract class PackageReferenceBase
    {
        /// <summary>
        /// Asset references that needs to be compiled even if not directly or indirectly referenced (useful for explicit code references).
        /// </summary>
        [DataMember(100)]
        public RootAssetCollection RootAssets { get; private set; } = new RootAssetCollection();
    }
}
