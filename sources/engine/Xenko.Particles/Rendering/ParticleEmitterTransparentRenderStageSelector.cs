// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;
using Xenko.Rendering;

namespace Xenko.Particles.Rendering
{
    public class ParticleEmitterTransparentRenderStageSelector : TransparentRenderStageSelector
    {
        public override void Process(RenderObject renderObject)
        {
            if (TransparentRenderStage != null && ((RenderGroupMask)(1U << (int)renderObject.RenderGroup) & RenderGroup) != 0)
            {
                var renderParticleEmitter = (RenderParticleEmitter)renderObject;
                var effectName = renderParticleEmitter.ParticleEmitter.Material.EffectName;

                renderObject.ActiveRenderStages[TransparentRenderStage.Index] = new ActiveRenderStage(effectName);
            }
        }
    }
}
