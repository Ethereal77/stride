// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Navigation
{
    /// <summary>
    /// Provides advanced settings to be passed to navigation mesh queries
    /// </summary>
    public struct NavigationQuerySettings
    {
        /// <summary>
        /// The default settings that are used when querying navigation meshes
        /// </summary>
        public static readonly NavigationQuerySettings Default = new NavigationQuerySettings
        {
            FindNearestPolyExtent = new Vector3(2.0f, 4.0f, 2.0f),
            MaxPathPoints = 1024,
        };

        /// <summary>
        /// Used as the extend for the find nearest poly bounding box used when scanning for a polygon corresponding to the given starting/ending position. 
        /// Making this bigger will allow you to find paths that allow the entity to start further away or higher from the navigation mesh bounds for example
        /// </summary>
        public Vector3 FindNearestPolyExtent;

        /// <summary>
        /// The maximum number of path points used internally and also the maximum number of output points
        /// </summary>
        public int MaxPathPoints;
    }
}
