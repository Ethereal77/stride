// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Particles.Updaters
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
