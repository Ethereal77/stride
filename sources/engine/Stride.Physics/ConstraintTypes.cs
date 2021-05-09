// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Physics
{
    public enum ConstraintTypes
    {
        /// <summary>
        ///     The translation vector of the matrix to create this will represent the pivot, the rest is ignored
        /// </summary>
        Point2Point,

        Hinge,

        Slider,

        ConeTwist,

        Generic6DoF,

        Generic6DoFSpring,

        /// <summary>
        ///     The translation vector of the matrix to create this will represent the axis, the rest is ignored
        /// </summary>
        Gear,
    }
}
