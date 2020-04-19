// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ComponentModel;

using Xenko.Engine;

namespace Xenko.Rendering
{
    public abstract class TransparentRenderStageSelector : RenderStageSelector
    {
        [DefaultValue(RenderGroupMask.All)]
        public RenderGroupMask RenderGroup { get; set; } = RenderGroupMask.All;

        [DefaultValue(null)]
        public RenderStage OpaqueRenderStage { get; set; }
        [DefaultValue(null)]
        public RenderStage TransparentRenderStage { get; set; }

        public string EffectName { get; set; }
    }
}
