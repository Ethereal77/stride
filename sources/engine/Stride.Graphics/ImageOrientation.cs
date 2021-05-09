// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
{
    /// <summary>
    /// Defines the possible rotations to apply on image regions.
    /// </summary>
    public enum ImageOrientation
    {
        /// <summary>
        /// The image region is taken as is.
        /// </summary>
        AsIs = 0,

        /// <summary>
        /// The image is rotated of the 90 degrees (clockwise) in the source texture.
        /// </summary>
        Rotated90 = 1,
    }
}
