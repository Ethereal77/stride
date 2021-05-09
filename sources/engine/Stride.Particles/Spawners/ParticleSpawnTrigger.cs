// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Particles.Spawners
{
    /// <summary>
    /// <see cref="ParticleSpawnTrigger"/> governs the condition under which particle emission is triggered for spawners like <see cref="SpawnerFromParent"/>
    /// </summary>
    public abstract class ParticleSpawnTrigger
    {
        /// <summary>
        /// Prepares fields accessors before the 
        /// </summary>
        /// <param name="pool"></param>
        public abstract void PrepareFromPool(ParticlePool pool);

        /// <summary>
        /// For positive values, shows if the condition was met and how much is the magnitude
        /// </summary>
        /// <param name="parentParticle">Parent particle whose attributes are used for testing</param>
        /// <returns>Zero if it hasn't been triggered, a positive value otherwise, which also indicates the magnitude of the trigger</returns>
        public abstract float HasTriggered(Particle parentParticle);

        /// <summary>
        /// Override to add the required fields to the parent emitter
        /// </summary>
        /// <param name="parentEmitter">Parent emitter to which required fields should be added</param>
        public virtual void AddRequiredParentFields(ParticleEmitter parentEmitter) { }

        /// <summary>
        /// Override to remove the required fields from the parent emitter
        /// </summary>
        /// <param name="parentEmitter">Parent emitter from which required fields should be removed</param>
        public virtual void RemoveRequiredParentFields(ParticleEmitter parentEmitter) { }
    }

    /// <inheritdoc/>
    public abstract class ParticleSpawnTrigger<T> : ParticleSpawnTrigger where T : struct
    {
        protected ParticleFieldAccessor<T> FieldAccessor;         
    }
}
