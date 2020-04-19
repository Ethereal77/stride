// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Mathematics;
using Xenko.Physics;

namespace Xenko.Navigation
{
    /// <summary>
    /// Data associated with static colliders for incremental building of navigation meshes
    /// </summary>
    public class StaticColliderData
    {
        public StaticColliderComponent Component;
        internal int ParameterHash = 0;
        internal bool Processed = false;
        internal NavigationMeshInputBuilder InputBuilder;
        internal NavigationMeshCachedObject Previous;

        /// <remarks>
        /// Planes are an exceptions to normal geometry since their size depends on the size of the bounding boxes in the scene, however we don't want to rebuild the whole scene, unless the actual shape of the plane changes
        /// </remarks>
        internal readonly List<Plane> Planes = new List<Plane>();
    }
}
