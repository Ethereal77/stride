// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2007-2011 SlimDX Group
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Mathematics
{
    /*
     * The enumerations defined in this file are in alphabetical order. When
     * adding new enumerations or renaming existing ones, please make sure
     * the ordering is maintained.
    */

    /// <summary>
    /// Describes the result of an intersection with a plane in three dimensions.
    /// </summary>
    public enum PlaneIntersectionType
    {
        /// <summary>
        /// The object is behind the plane.
        /// </summary>
        Back,

        /// <summary>
        /// The object is in front of the plane.
        /// </summary>
        Front,

        /// <summary>
        /// The object is intersecting the plane.
        /// </summary>
        Intersecting,
    }
}
