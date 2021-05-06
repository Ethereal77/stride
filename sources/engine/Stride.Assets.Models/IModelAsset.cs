// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Stride.Assets.Models
{
    /// <summary>
    ///   Defines the interface of an object representing an Asset containing a Model.
    /// </summary>
    public interface IModelAsset
    {
        /// <summary>
        ///   Gets the list of materials in the model.
        /// </summary>
        /// <userdoc>The list of materials in the model.</userdoc>
        List<ModelMaterial> Materials { get; }
    }
}
