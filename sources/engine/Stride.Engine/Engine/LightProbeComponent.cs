// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Annotations;
using Xenko.Core.Collections;
using Xenko.Core.Mathematics;
using Xenko.Engine.Design;
using Xenko.Rendering.LightProbes;

namespace Xenko.Engine
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
