// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
{
    /// <summary>
    /// Defines keys associated with mesh used for compiling assets.
    /// </summary>
    public class MeshKeys
    {
        /// <summary>
        /// When compiling effect with an EffectLibraryAsset (xkfxlib), set it to true to allow permutation based on the 
        /// parameters of all meshes.
        /// </summary>
        /// TODO: allow permutation for a specific mesh
        /// <userdoc>
        /// Use the mesh parameters to generate effects
        /// </userdoc>
        public static readonly ValueParameterKey<bool> UseParameters = ParameterKeys.NewValue<bool>();
    }
}
