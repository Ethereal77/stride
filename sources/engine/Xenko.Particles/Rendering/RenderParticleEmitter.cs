// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;

namespace Xenko.Particles.Rendering
{
    /// <summary>
    /// Defines a particle emitter to render.
    /// </summary>
    public class RenderParticleEmitter : RenderObject
    {
        public RenderParticleSystem RenderParticleSystem;

        public ParticleEmitter ParticleEmitter;
        internal ParticleEmitterRenderFeature.ParticleMaterialInfo ParticleMaterialInfo;

        public Color4 Color;

        public bool HasVertexBufferChanged;
        public int VertexSize;
        public int VertexCount;
    }
}
