// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Defines the dimension of a texture.
    /// </summary>
    [DataContract]
    public enum TextureDimension
    {
        /// <summary>
        /// The texture dimension is 1D.
        /// </summary>
        Texture1D,

        /// <summary>
        /// The texture dimension is 2D.
        /// </summary>
        Texture2D,

        /// <summary>
        /// The texture dimension is 3D.
        /// </summary>
        Texture3D,

        /// <summary>
        /// The texture dimension is a CubeMap.
        /// </summary>
        TextureCube,
    }
}
