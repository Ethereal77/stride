// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Engine;
using Stride.Rendering;

namespace Stride.Particles.Rendering
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
