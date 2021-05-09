// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    /// <summary>
    /// Defines the faces of a cube map for <see cref="TextureCube"/>.
    /// </summary>
    public enum CubeMapFace
    {
        /// <summary>
        /// Positive x-face of the cube map.
        /// </summary>
        PositiveX,
        /// <summary>
        /// Negative x-face of the cube map.
        /// </summary>
        NegativeX,
        /// <summary>
        /// Positive y-face of the cube map.
        /// </summary>
        PositiveY,
        /// <summary>
        /// Negative y-face of the cube map.
        /// </summary>
        NegativeY,
        /// <summary>
        /// Positive z-face of the cube map.
        /// </summary>
        PositiveZ,
        /// <summary>
        /// Negative z-face of the cube map.
        /// </summary>
        NegativeZ,
    }
}
