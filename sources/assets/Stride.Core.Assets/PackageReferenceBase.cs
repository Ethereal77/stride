// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;

namespace Stride.Core.Assets
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
