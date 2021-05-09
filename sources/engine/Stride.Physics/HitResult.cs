// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.InteropServices;

using Stride.Core.Mathematics;
using Stride.Engine;

namespace Stride.Physics
{
    /// <summary>
    /// The result of a Physics Raycast or ShapeSweep operation
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct HitResult
    {
        public Vector3 Normal;

        public Vector3 Point;

        public float HitFraction;

        public bool Succeeded;

        /// <summary>
        /// The Collider hit if Succeeded
        /// </summary>
        public PhysicsComponent Collider;
    }
}
