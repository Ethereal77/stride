// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Xenko.Core.Mathematics;
using Xenko.Engine;

namespace Xenko.Physics
{
    public struct ContactPoint
    {
        public PhysicsComponent ColliderA;
        public PhysicsComponent ColliderB;
        public float Distance;
        public Vector3 Normal;
        public Vector3 PositionOnA;
        public Vector3 PositionOnB;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct NativeContactPoint
    {
        internal readonly IntPtr ColliderA;
        internal readonly IntPtr ColliderB;
        public readonly float Distance;
        public readonly Vector3 Normal;
        public readonly Vector3 PositionOnA;
        public readonly Vector3 PositionOnB;
    }

    public class ContactPointEqualityComparer : EqualityComparer<ContactPoint>
    {
        /// <summary>
        /// Gets the default.
        /// </summary>
        public static new readonly ContactPointEqualityComparer Default = new ContactPointEqualityComparer();

        /// <inheritdoc/>
        public override bool Equals(ContactPoint x, ContactPoint y)
        {
            return (x.ColliderA == y.ColliderA && x.ColliderB == y.ColliderB) || (x.ColliderA == y.ColliderB && x.ColliderB == y.ColliderA);
        }

        /// <inheritdoc/>
        public override int GetHashCode(ContactPoint obj)
        {
            return 397 * obj.ColliderA.GetHashCode() * obj.ColliderB.GetHashCode();
        }
    }
}
