// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Assets.Textures
{
    /// <summary>
    /// Gives a hint to the texture compressor on the kind of textures to select the appropriate compression format depending
    /// on the HW Level and platform.
    /// </summary>
    [DataContract("TextureHint")]
    public enum TextureHint
    {
        /// <summary>
        /// The texture is using the full color.
        /// </summary>
        Color,

        /// <summary>
        /// The texture is a grayscale.
        /// </summary>
        Grayscale,

        /// <summary>
        /// The texture is a normal map.
        /// </summary>
        NormalMap
    }
}
