﻿// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Engine;

namespace Stride.Rendering.Shadows
{
    public class ShadowMapRenderStageSelector : RenderStageSelector
    {
        public RenderStage ShadowMapRenderStage { get; set; }
        public string EffectName { get; set; }

        [DefaultValue(RenderGroupMask.All)]
        public RenderGroupMask RenderGroup { get; set; } = RenderGroupMask.All;

        public override void Process(RenderObject renderObject)
        {
            if (ShadowMapRenderStage != null && ((RenderGroupMask)(1U << (int)renderObject.RenderGroup) & RenderGroup) != 0)
            {
                var renderMesh = (RenderMesh)renderObject;

                // Only handle non-transparent meshes
                //if (!renderMesh.MaterialPass.HasTransparency)
                {
                    if (renderMesh.IsShadowCaster)
                        renderMesh.ActiveRenderStages[ShadowMapRenderStage.Index] = new ActiveRenderStage(EffectName);
                }
            }
        }
    }
}
