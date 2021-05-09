// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Engine.Events;
using Stride.Physics;

using FirstPersonShooter.Player;

namespace FirstPersonShooter
{
    /// <summary>
    /// Will spawn some visual effects when the gun shoots
    /// </summary>
    public class EffectController : TriggerScript
    {      
        private readonly EventReceiver<WeaponFiredResult> weaponFiredEvent = new EventReceiver<WeaponFiredResult>(WeaponScript.WeaponFired);

        public override async Task Execute()
        {
            while (Game.IsRunning)
            {
                var target = await weaponFiredEvent.ReceiveAsync();

                if (target.DidFire)
                    SpawnEvent("MuzzleFlash", Entity, Matrix.Identity);

                if (target.DidHit)
                    SpawnEvent("BulletImpact", null, Matrix.RotationQuaternion(Quaternion.BetweenDirections(Vector3.UnitY, target.HitResult.Normal)) * Matrix.Translation(target.HitResult.Point));

                var rigidBody = target.HitResult.Collider as RigidbodyComponent;
                if (rigidBody != null)
                {
                    var rand = new Random();
                    SpawnEvent("DamagedTrail", rigidBody.Entity, Matrix.Translation(new Vector3((float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f)));
                }
            }
        }        
    }
}
