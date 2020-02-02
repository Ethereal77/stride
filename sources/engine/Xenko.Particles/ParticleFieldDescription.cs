// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Particles
{
    public abstract class ParticleFieldDescription
    {
        private readonly int hashCode;

        protected ParticleFieldDescription(string name)
        {
            Name = name;
            hashCode = name?.GetHashCode() ?? 0;
            FieldSize = 0;
        }

        public override int GetHashCode() => hashCode;

        public int FieldSize { get; protected set; }

        public string Name { get; }
    }

    public class ParticleFieldDescription<T> : ParticleFieldDescription where T : struct
    {
        public ParticleFieldDescription(string name)
            : base(name)
        {
            FieldSize = ParticleUtilities.AlignedSize(Utilities.SizeOf<T>(), 4);
        }

        public ParticleFieldDescription(string name, T defaultValue)
            : this(name)
        {
            DefaultValue = defaultValue;
        }

        public T DefaultValue { get; }
    }
}
