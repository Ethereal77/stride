// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Physics
{
    public class GearConstraint : Constraint
    {
        internal BulletSharp.GearConstraint InternalGearConstraint;

        /// <summary>
        /// Gets or sets the axis a.
        /// </summary>
        /// <value>
        /// The axis a.
        /// </value>
        public Vector3 AxisA
        {
            get { return InternalGearConstraint.AxisA; }
            set { InternalGearConstraint.AxisA = value; }
        }

        /// <summary>
        /// Gets or sets the axis b.
        /// </summary>
        /// <value>
        /// The axis b.
        /// </value>
        public Vector3 AxisB
        {
            get { return InternalGearConstraint.AxisB; }
            set { InternalGearConstraint.AxisB = value; }
        }

        /// <summary>
        /// Gets or sets the ratio.
        /// </summary>
        /// <value>
        /// The ratio.
        /// </value>
        public float Ratio
        {
            get { return InternalGearConstraint.Ratio; }
            set { InternalGearConstraint.Ratio = value; }
        }
    }
}
