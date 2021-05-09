// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Assets;
using Stride.Core;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.Assets.Models
{
    /// <summary>
    /// A model asset that is generated from a prefab, combining and merging meshes by materials and layout.
    /// </summary>
    [DataContract("PrefabModelAsset")]
    [AssetDescription(FileExtension)]
    [AssetContentType(typeof(Model))]
    [Display((int)AssetDisplayPriority.Models + 60, "Prefab model")]
    [AssetFormatVersion(StrideConfig.PackageName, CurrentVersion, "2.0.0.0")]
    public sealed class PrefabModelAsset : Asset, IModelAsset
    {
        private const string CurrentVersion = "2.0.0.0";

        /// <summary>
        /// The default file extension used by the <see cref="ProceduralModelAsset"/>.
        /// </summary>
        public const string FileExtension = ".sdprefabmodel";

        /// <inheritdoc/>
        [DataMemberIgnore] // materials are not exposed in prefab models
        public List<ModelMaterial> Materials { get; } = new List<ModelMaterial>();

        [DataMember]
        public Prefab Prefab { get; set; }
    }
}
