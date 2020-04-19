// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering.Materials
{
    /// <summary>
    /// Enumerates the different possible material shader stages.
    /// </summary>
    public enum MaterialShaderStage
    {
        /// <summary>
        /// The vertex shader
        /// </summary>
        Vertex,
        
        /// <summary>
        /// The domain shader
        /// </summary>
        Domain,

        /// <summary>
        /// The pixel shader
        /// </summary>
        Pixel,
    }
}
