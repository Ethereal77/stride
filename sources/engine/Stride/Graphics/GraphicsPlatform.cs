// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Graphics
{
    /// <summary>
    /// The graphics platform.
    /// </summary>
    [DataContract("GraphicsPlatform")]
    public enum GraphicsPlatform
    {
        /// <summary>
        /// The Null Renderer / Shader.
        /// </summary>
        Null,

        /// <summary>
        /// Direct3D 11 Renderer / HLSL Shader.
        /// </summary>
        Direct3D11,

        /// <summary>
        /// Direct3D 12 Renderer / HLSL Shader.
        /// </summary>
        Direct3D12
    }
}
