// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Assets.Models
{
    /// <summary>
    /// This interface represents an asset containing a model.
    /// </summary>
    public interface IModelAsset
    {
        /// <summary>
        /// The materials.
        /// </summary>
        /// <userdoc>
        /// The list of materials in the model.
        /// </userdoc>
        List<ModelMaterial> Materials { get; }
    }
}
