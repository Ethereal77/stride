﻿// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Engine;

namespace Xenko.Rendering
{
    public class SimpleGroupToRenderStageSelector : RenderStageSelector
    {
        [DefaultValue(RenderGroupMask.All)]
        public RenderGroupMask RenderGroup { get; set; } = RenderGroupMask.All;

        public RenderStage RenderStage { get; set; }

        public string EffectName { get; set; }

        public override void Process(RenderObject renderObject)
        {
            if (RenderStage != null && ((RenderGroupMask)(1U << (int)renderObject.RenderGroup) & RenderGroup) != 0)
            {
                renderObject.ActiveRenderStages[RenderStage.Index] = new ActiveRenderStage(EffectName);
            }
        }
    }
}
