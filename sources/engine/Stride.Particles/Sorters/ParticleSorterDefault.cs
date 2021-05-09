// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Particles.Sorters
{
    /// <summary>
    /// The default sorter doesn not sort the particles, but only passes them directly to the renderer
    /// </summary>
    public class ParticleSorterDefault : IParticleSorter
    {
        /// <summary>
        /// Target <see cref="ParticlePool"/> to iterate and sort
        /// </summary>
        protected readonly ParticlePool ParticlePool;

        public ParticleSorterDefault(ParticlePool pool)
        {
            ParticlePool = pool;
        }

        public ParticleList GetSortedList(Vector3 depth)
        {
            return new ParticleList(ParticlePool, ParticlePool.LivingParticles);
        }

        /// <summary>
        /// The default sorter does not allocate any resources so there is nothing to free
        /// </summary>
        /// <param name="sortedList">Reference to the <see cref="ParticleList"/> to be freed</param>
        public void FreeSortedList(ref ParticleList sortedList) { }
    }
}
