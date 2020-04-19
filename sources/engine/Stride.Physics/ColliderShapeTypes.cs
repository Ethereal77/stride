// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Physics
{
    public enum ColliderShapeTypes
    {
        /// <summary>
        ///     3D and 2D ( a plane )
        /// </summary>
        Box,

        /// <summary>
        ///     3D and 2D ( a circle )
        /// </summary>
        Sphere,

        /// <summary>
        ///     3D only
        /// </summary>
        Cylinder,

        /// <summary>
        ///     3D and 2D
        /// </summary>
        Capsule,

        ConvexHull,

        Compound,

        StaticPlane,

        StaticMesh,

        Cone,

        Heightfield,
    }
}
