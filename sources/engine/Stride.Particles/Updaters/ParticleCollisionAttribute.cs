// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Particles.Updaters
{
    public struct ParticleCollisionAttribute
    {
        public static ParticleCollisionAttribute Empty = new ParticleCollisionAttribute { flags = 0 };

        private uint flags;

        public ParticleCollisionAttribute(ParticleCollisionAttribute other)
        {
            flags = other.flags;
        }

        private const uint FlagsHasColided = 0x1 << 0;

        public bool HasColided
        {
            get { return (flags & FlagsHasColided) > 0; }
            set
            {
                if (value)
                    flags |= FlagsHasColided;
                else
                    flags &= ~FlagsHasColided;
            }
        }
    }
}
