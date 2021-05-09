// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Specify how to swizzle a vector.
    /// </summary>
    public enum SwizzleMode
    {
        /// <summary>
        /// Take the vector as is.
        /// </summary>
        [Display("Default")]
        None = 0,

        /// <summary>
        /// Take the only the red component of the vector.
        /// </summary>
        [Display("Grayscale (alpha)")]
        RRRR = 1,

        /// <summary>
        /// Reconstructs the Z(B) component from R and G.
        /// </summary>
        [Display("Normal map")]
        NormalMap = 2,

        /// <summary>
        /// Take the only the red component of the vector, but keeps the object opaque
        /// </summary>
        [Display("Grayscale (opaque)")]
        RRR1 = 3,

        /// <summary>
        /// Take the only the x component of the vector.
        /// </summary>
        [Display("Grayscale (alpha)")]
        XXXX = RRRR,
    }
}
