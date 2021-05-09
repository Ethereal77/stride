// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Particles.Updaters;

namespace Stride.Particles.Spawners
{
    /// <summary>
    /// <see cref="ParticleSpawnTriggerCollision"/> triggers when the parent particle collides with a surface
    /// </summary>
    [DataContract("ParticleSpawnTriggerCollision")]
    [Display("On Hit")]
    public class ParticleSpawnTriggerCollision : ParticleSpawnTrigger<ParticleCollisionAttribute>
    {
        public override void PrepareFromPool(ParticlePool pool)
        {
            if (pool == null)
            {
                FieldAccessor = ParticleFieldAccessor<ParticleCollisionAttribute>.Invalid();
                return;
            }

            FieldAccessor = pool.GetField(ParticleFields.CollisionControl);
        }

        public unsafe override float HasTriggered(Particle parentParticle)
        {
            if (!FieldAccessor.IsValid())
                return 0f;

            var collisionAttribute = (*((ParticleCollisionAttribute*)parentParticle[FieldAccessor]));
            return (collisionAttribute.HasColided) ? 1f : 0f;
        }
    }
}
