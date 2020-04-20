// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Identifies the set of supported devices for the graphics subsystem based on device capabilities.
    /// </summary>
    [DataContract("GraphicsProfile")]
    public enum GraphicsProfile
    {
        /// <summary>
        /// DirectX11 support (HLSL 5.0, Compute Shaders, Domain/Hull Shaders)
        /// </summary>
        [Display("Direct3D 11.0")]
        Level_11_0 = 0xB000,

        /// <summary>
        /// DirectX11.1 support (HLSL 5.0, Compute Shaders, Domain/Hull Shaders)
        /// </summary>
        [Display("Direct3D 11.1")]
        Level_11_1 = 0xB100,

        /// <summary>
        /// DirectX11.2 support (HLSL 5.0, Compute Shaders, Domain/Hull Shaders)
        /// </summary>
        [Display("Direct3D 11.2")]
        Level_11_2 = 0xB200,
    }
}
