// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace Stride.Particles.Sorters
{
    public struct ParticleList : IEnumerable
    {
        private readonly SortedParticle[] sortedList;
        private readonly int listCapacity;
        private readonly ParticlePool pool;

        public ParticleList(ParticlePool pool, int capacity, SortedParticle[] list = null)
        {
            this.pool = pool;
            this.listCapacity = capacity;
            this.sortedList = list;
        }

        public void Free(ConcurrentArrayPool<SortedParticle> sortedArrayPool)
        {
            if (sortedList != null)
            {
                sortedArrayPool.Free(sortedList);
            }
        }

        /// <summary>
        /// Returns a particle field accessor for the contained <see cref="ParticlePool"/>
        /// </summary>
        /// <typeparam name="T">Type data for the field</typeparam>
        /// <param name="fieldDesc">The field description</param>
        /// <returns></returns>
        public ParticleFieldAccessor<T> GetField<T>(ParticleFieldDescription<T> fieldDesc) where T : struct => pool.GetField<T>(fieldDesc);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(pool, listCapacity, sortedList);
        }

        public struct Enumerator : IEnumerator<Particle>
        {
            private readonly SortedParticle[] sortedList;
            private readonly int listCapacity;
            private readonly ParticlePool pool;

            private int index;

            internal Enumerator(ParticlePool pool, int capacity, SortedParticle[] list)
            {
                sortedList = list;
                listCapacity = capacity;
                index = -1;
                Current = Particle.Invalid();
                this.pool = pool;
            }

            /// <inheritdoc />
            public void Reset()
            {
                index = -1;
                Current = Particle.Invalid();
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                bool moveNext = (++index < listCapacity);
                Current = (moveNext) ? ((sortedList != null) ? sortedList[index].Particle : pool.FromIndex(index)) : Particle.Invalid();
                return moveNext;
            }

            /// <inheritdoc />
            public void Dispose() { }

            public Particle Current { get; private set; }

            object IEnumerator.Current => Current;
        }
    }
}
