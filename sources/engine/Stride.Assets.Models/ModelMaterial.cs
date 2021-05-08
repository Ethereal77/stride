// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Rendering;

namespace Stride.Assets.Models
{
    /// <summary>
    ///   Represents a <see cref="Stride.Rendering.MaterialInstance"/> in a <see cref="ModelAsset"/>.
    /// </summary>
    [DataContract]
    public class ModelMaterial
    {
        /// <summary>
        ///   Gets or sets the material slot name in a <see cref="ModelAsset"/>.
        /// </summary>
        /// <value>The material slot name.</value>
        /// <userdoc>The name of the material as written in the imported model and the reference to the corresponding material asset.</userdoc>
        [DataMember(10)]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the material stored in this slot.
        /// </summary>
        [DataMember(20)]
        public MaterialInstance MaterialInstance { get; set; }
    }
}
