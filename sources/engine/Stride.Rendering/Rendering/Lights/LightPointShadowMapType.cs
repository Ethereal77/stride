// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Rendering.Lights
{
    /// <summary>
    /// Possible types of shadow mapping for point lights
    /// </summary>
    [DataContract]
    public enum LightPointShadowMapType
    {
        /// <summary>
        /// Renders the scene only twice to 2 hemisphere textures, might look distorted or generate artifacts with low-poly shadow casters
        /// </summary>
        DualParaboloid,
        /// <summary>
        /// Renders the scene to 6 faces of a cube, provides more stable shadow maps with the tradoff of having to render the scene 6 times
        /// </summary>
        CubeMap,
    }
}
