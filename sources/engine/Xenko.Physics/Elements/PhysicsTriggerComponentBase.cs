// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Core;
using Xenko.Engine;

namespace Xenko.Physics
{
    [DataContract("PhysicsTriggerComponentBase")]
    [Display("PhysicsTriggerComponentBase")]
    public abstract class PhysicsTriggerComponentBase : PhysicsComponent
    {
        private bool isTrigger;

        [DataMember(71)]
        public bool IsTrigger
        {
            get
            {
                return isTrigger;
            }
            set
            {
                isTrigger = value;

                if (NativeCollisionObject == null) return;

                if (isTrigger)
                {
                    NativeCollisionObject.CollisionFlags |= BulletSharp.CollisionFlags.NoContactResponse;
                }
                else
                {
                    if ((NativeCollisionObject.CollisionFlags & BulletSharp.CollisionFlags.NoContactResponse) != 0)
                    {
                        NativeCollisionObject.CollisionFlags ^= BulletSharp.CollisionFlags.NoContactResponse;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets if this element is enabled in the physics engine
        /// </summary>
        /// <value>
        /// true, false
        /// </value>
        /// <userdoc>
        /// If this element is enabled in the physics engine
        /// </userdoc>
        [DataMember(-10)]
        [DefaultValue(true)]
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;

                if (NativeCollisionObject == null) return;

                if (value && isTrigger)
                {
                    //We still have to add this flag if we are actively a trigger
                    NativeCollisionObject.CollisionFlags |= BulletSharp.CollisionFlags.NoContactResponse;
                }
            }
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            //set pre-set post deserialization properties
            IsTrigger = isTrigger;
        }
    }
}
