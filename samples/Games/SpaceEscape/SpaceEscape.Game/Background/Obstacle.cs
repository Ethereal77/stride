// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Mathematics;
using Xenko.Engine;

namespace SpaceEscape.Background
{
    /// <summary>
    /// The class contains information needed to describe a collidable object.
    /// </summary>
    public class Obstacle
    {
        /// <summary>
        /// The list of bounding boxes used to determine the collision with the obstacle.
        /// </summary>
        public List<BoundingBox> BoundingBoxes = new List<BoundingBox>(); 

        /// <summary>
        /// The entity representing the collidable object.
        /// </summary>
        public Entity Entity { get; set; }
    }
}
