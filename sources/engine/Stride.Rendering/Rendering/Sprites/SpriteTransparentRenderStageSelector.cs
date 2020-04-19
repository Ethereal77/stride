// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;

namespace Xenko.Rendering.Sprites
{
    public class SpriteTransparentRenderStageSelector : TransparentRenderStageSelector
    {
        public override void Process(RenderObject renderObject)
        {
            if (((RenderGroupMask)(1U << (int)renderObject.RenderGroup) & RenderGroup) != 0)
            {
                var renderSprite = (RenderSprite)renderObject;

                var renderStage = renderSprite.Sprite.IsTransparent ? TransparentRenderStage : OpaqueRenderStage;
                if (renderStage != null)
                    renderObject.ActiveRenderStages[renderStage.Index] = new ActiveRenderStage(EffectName);
            }
        }
    }
}
