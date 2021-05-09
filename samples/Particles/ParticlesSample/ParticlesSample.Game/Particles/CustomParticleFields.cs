// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Particles;

namespace ParticlesSample.Particles
{
    public static class CustomParticleFields
    {
        /// <summary>
        /// Custom field for our particle, which defines non-uniform dimensions in 2D space
        /// </summary>
        public static readonly ParticleFieldDescription<Vector2> RectangleXY = new ParticleFieldDescription<Vector2>("RectangleXY", new Vector2(1, 1));
    }
}
