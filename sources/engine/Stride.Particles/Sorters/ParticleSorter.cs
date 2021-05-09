// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Mathematics;

namespace Stride.Particles.Sorters
{
    public struct SortedParticle : IComparable<SortedParticle>
    {
        public readonly Particle Particle;
        public readonly float SortIndex;         // TODO Maybe use a Int32 key rather than float?

        public SortedParticle(Particle particle, float sortIndex)
        {
            Particle = particle;
            SortIndex = sortIndex;
        }

        int IComparable<SortedParticle>.CompareTo(SortedParticle other)
        {
            return (SortIndex < other.SortIndex) ? -1 : (SortIndex > other.SortIndex) ? 1 : 0;
        }

        public static bool operator <(SortedParticle left, SortedParticle right) => (left.SortIndex < right.SortIndex);

        public static bool operator >(SortedParticle left, SortedParticle right) => (left.SortIndex > right.SortIndex);

        public static bool operator <=(SortedParticle left, SortedParticle right) => (left.SortIndex <= right.SortIndex);

        public static bool operator >=(SortedParticle left, SortedParticle right) => (left.SortIndex >= right.SortIndex);

        public static bool operator ==(SortedParticle left, SortedParticle right) => (left.SortIndex == right.SortIndex);

        public static bool operator !=(SortedParticle left, SortedParticle right) => (left.SortIndex != right.SortIndex);

        public override bool Equals(object obj)
        {
            if (!(obj is SortedParticle))
                return false;

            var other = (SortedParticle)obj;
            return (this == other);
        }

        public override int GetHashCode()
        {
            return SortIndex.GetHashCode();
        }
    }

    public interface IParticleSorter
    {
        ParticleList GetSortedList(Vector3 depth);

        void FreeSortedList(ref ParticleList sortedList);
    }
}
