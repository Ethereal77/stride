// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering
{
    /// <summary>
    /// Describe the different tessellation methods used in Stride.
    /// </summary>
    [Flags]
    public enum StrideTessellationMethod
    {
        /// <summary>
        /// No tessellation
        /// </summary>
        None = 0,

        /// <summary>
        /// Flat tessellation. Also known as dicing tessellation.
        /// </summary>
        Flat = 1,

        /// <summary>
        /// Point normal tessellation.
        /// </summary>
        PointNormal = 1,

        /// <summary>
        /// Adjacent edge average.
        /// </summary>
        AdjacentEdgeAverage = 2,
    }
}
