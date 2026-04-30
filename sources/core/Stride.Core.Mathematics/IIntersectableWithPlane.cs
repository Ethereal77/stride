// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2022 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Mathematics
{
    /// <summary>
    /// Allows to determine intersections with a <see cref="Stride.Core.Mathematics.Plane"/>.
    /// </summary>
    public interface IIntersectableWithPlane
    {
        /// <summary>
        /// Determines if there is an intersection between the current object and a <see cref="Stride.Core.Mathematics.Plane"/>.
        /// </summary>
        /// <param name="plane">The plane to test.</param>
        /// <returns>Whether the two objects intersected.</returns>
        public PlaneIntersectionType Intersects(ref Plane plane);
    }
}
