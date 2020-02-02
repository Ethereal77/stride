// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;

namespace Xenko.Physics
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
