// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2007-2011 SlimDX Group
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Mathematics
{
    /// <summary>
    /// Describes how one bounding volume contains another.
    /// </summary>
    public enum ContainmentType
    {
        /// <summary>
        /// The two bounding volumes don't intersect at all.
        /// </summary>
        Disjoint,

        /// <summary>
        /// One bounding volume completely contains another.
        /// </summary>
        Contains,

        /// <summary>
        /// The two bounding volumes overlap.
        /// </summary>
        Intersects,
    }
}
