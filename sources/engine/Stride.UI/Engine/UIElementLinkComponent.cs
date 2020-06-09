// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Engine.Design;
using Stride.Engine.Processors;

namespace Stride.Engine
{
    [DataContract("UIElementLinkComponent")]
    [Display("UI element link", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(UIElementLinkProcessor))]
    [ComponentOrder(1600)]
    [ComponentCategory("UI")]
    public sealed class UIElementLinkComponent : EntityComponent
    {
        /// <summary>
        ///   Gets or sets the UI component which contains the hierarchy to use.
        /// </summary>
        /// <value>The UI component which contains the hierarchy to use.</value>
        /// <userdoc>
        ///   The reference to the target entity to which to attach the current entity. If <c>null</c>,
        ///   the parent will be used.
        /// </userdoc>
        [Display("Target (Parent if not set)")]
        public UIComponent Target { get; set; }

        /// <summary>
        ///   Gets or sets the camera component towards which to orient the UI component if it is a billboard.
        /// </summary>
        /// <value>
        ///   The camera component which is required if the UI component is a billboard.
        /// </value>
        /// <userdoc>
        ///   The reference to the target camera used to render the component. It is only required in case the
        ///   parent UI component is a billboard.
        /// </userdoc>
        [Display("Camera (if billboard)")]
        public CameraComponent Camera { get; set; }


        /// <summary>
        ///   Gets or sets the name of the element.
        /// </summary>
        /// <value>
        ///   The name of the element.
        /// </value>
        /// <userdoc>The name of the node of the model of the target entity to which to attach the current entity.</userdoc>
        public string NodeName { get; set; }
    }
}
