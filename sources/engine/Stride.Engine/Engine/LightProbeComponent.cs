// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Engine.Design;
using Stride.Rendering.LightProbes;

namespace Stride.Engine
{
    [DataContract("LightProbeComponent")]
    [Display("Light probe", Expand = ExpandRule.Once)]
    [DefaultEntityComponentRenderer(typeof(LightProbeProcessor))]
    [ComponentOrder(15000)]
    [ComponentCategory("Lights")]
    public class LightProbeComponent : EntityComponent
    {
        [Display(Browsable = false)]
        [NonIdentifiableCollectionItems]
        public FastList<Color3> Coefficients { get; set; }
    }
}
