// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;

namespace PhysicsSample
{
    public class AutoResetRigidbody : AsyncScript
    {
        private Vector3 startLocation;
        private Quaternion startRotation;

        public Entity TriggerEntity;

        public override async Task Execute()
        {
            startLocation = Entity.Transform.Position;
            startRotation = Entity.Transform.Rotation;

            //grab a reference to the falling sphere's rigidbody
            var rigidBody = Entity.Get<RigidbodyComponent>();

            var triggerKey = TriggerEntity.Get<Trigger>().TriggerEvent;
            var receiver = new EventReceiver<bool>(triggerKey);

            while (!CancellationToken.IsCancellationRequested)
            {
                var state = await receiver.ReceiveAsync();
				
				if (CancellationToken.IsCancellationRequested)
                    return;
				
                if (state)
                {
                    //switch to dynamic and awake the rigid body
                    rigidBody.RigidBodyType = RigidBodyTypes.Dynamic;
                    rigidBody.Activate(true); //need to awake to object
                }
                else
                {
                    //when out revert to kinematic and old starting position
                    rigidBody.RigidBodyType = RigidBodyTypes.Kinematic;
                    //reset position
                    Entity.Transform.Position = startLocation;
                    Entity.Transform.Rotation = startRotation;
                    Entity.Transform.UpdateWorldMatrix();
                    Entity.Get<PhysicsComponent>().UpdatePhysicsTransformation();
                }
            }
        }
    }
}
