// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Particles.Initializers
{
    /// <summary>
    /// Spawn order can be additionally subdivided in groups
    /// </summary>
    public static class SpawnOrderConst
    {
        public const int GroupBitOffset = 16;
        public const uint GroupBitMask = 0xFFFF0000;
        public const uint AuxiliaryBitMask = 0x0000FFFF;
        public const int LargeGroupBitOffset = 20;
        public const uint LargeGroupBitMask = 0xFFF00000;
        public const uint LargeAuxiliaryBitMask = 0x000FFFFF;
    }
}
