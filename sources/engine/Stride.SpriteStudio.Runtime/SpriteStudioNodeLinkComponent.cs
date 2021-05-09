// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Engine;
using Stride.Engine.Design;

namespace Stride.SpriteStudio.Runtime
{
    [DataContract("SpriteStudioNodeLinkComponent")]
    [Display("SpriteStudio node link", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(SpriteStudioNodeLinkProcessor))]
    [ComponentOrder(1400)]
    [ComponentCategory("Sprites")]
    public sealed class SpriteStudioNodeLinkComponent : EntityComponent
    {
        /// <summary>
        /// Gets or sets the model which contains the hierarchy to use.
        /// </summary>
        /// <value>
        /// The model which contains the hierarchy to use.
        /// </value>
        /// <userdoc>The reference to the target entity to attach the current entity to. If null, parent will be used.</userdoc>
        [Display("Target (Parent if not set)")]
        public SpriteStudioComponent Target { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        /// <userdoc>The name of node of the model of the target entity to attach the current entity to.</userdoc>
        public string NodeName { get; set; }
    }
}
