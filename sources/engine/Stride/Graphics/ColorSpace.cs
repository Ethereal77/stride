// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Defines the different color spaces that can be used for textures, materials, lighting, etc.
    /// </summary>
    [DataContract("ColorSpace")]
    public enum ColorSpace
    {
        /// <summary>
        ///   Use a linear colorspace.
        /// </summary>
        Linear,

        /// <summary>
        ///   Use a gamma colorspace.
        /// </summary>
        Gamma,
    }
}
