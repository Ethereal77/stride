// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Defines the dimensions of a texture.
    /// </summary>
    [DataContract]
    public enum TextureDimension
    {
        /// <summary>
        ///   The texture is a one-dimensional buffer.
        /// </summary>
        Texture1D,

        /// <summary>
        ///   The texture is a two-dimensional image.
        /// </summary>
        Texture2D,

        /// <summary>
        ///   The texture is a three-dimensional volume texture.
        /// </summary>
        Texture3D,

        /// <summary>
        ///   The texture is an array of six two-dimensional images forming a cube.
        /// </summary>
        TextureCube
    }
}
