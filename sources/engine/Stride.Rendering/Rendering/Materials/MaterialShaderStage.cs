// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Materials
{
    /// <summary>
    ///   Defines the different possible material shader stages.
    /// </summary>
    public enum MaterialShaderStage
    {
        /// <summary>
        ///   The vertex shader
        /// </summary>
        Vertex,

        /// <summary>
        ///   The domain shader
        /// </summary>
        Domain,

        /// <summary>
        ///   The pixel shader
        /// </summary>
        Pixel
    }
}
