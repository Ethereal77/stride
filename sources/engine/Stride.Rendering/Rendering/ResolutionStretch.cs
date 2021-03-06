// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering
{
    /// <summary>
    ///   Defines the different ways to interpret a visual resolution value.
    /// </summary>
    [DataContract]
    public enum ResolutionStretch
    {
        /// <summary>
        ///   The resolution is determined by the width, height, and depth of the field.
        /// </summary>
        FixedWidthFixedHeight,

        /// <summary>
        ///   The resolution is determined by the width, the ratio of the target, and the depth.
        /// </summary>
        FixedWidthAdaptableHeight,

        /// <summary>
        ///   The resolution is determined by the height, the ratio of the target, and the depth.
        /// </summary>
        FixedHeightAdaptableWidth,
    }
}
