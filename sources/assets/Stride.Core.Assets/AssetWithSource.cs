// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;
using Stride.Core.IO;

namespace Stride.Core.Assets
{
    /// <summary>
    /// An asset that references a source file used during the compilation of the asset.
    /// </summary>
    [DataContract]
    public abstract class AssetWithSource : Asset, IAssetWithSource
    {
        /// <summary>
        /// Gets or sets the source file of this asset.
        /// </summary>
        /// <value>The source.</value>
        /// <userdoc>
        /// The source file of this asset.
        /// </userdoc>
        [DataMember(-50)]
        [DefaultValue(null)]
        [SourceFileMember(false)]
        public UFile Source { get; set; } = new UFile("");

        [DataMemberIgnore]
        public override UFile MainSource => Source;
    }
}
