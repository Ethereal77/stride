// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.ComponentModel;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine.Design;
using Stride.Rendering;
using Stride.Rendering.UI;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a component that adds an <see cref="UIPage"/> to an <see cref="Entity"/>.
    /// </summary>
    [DataContract("UIComponent")]
    [Display("UI", Expand = ExpandRule.Once)]
    [DefaultEntityComponentRenderer(typeof(UIRenderProcessor))]
    [ComponentOrder(9800)]
    [ComponentCategory("UI")]
    public sealed class UIComponent : ActivableEntityComponent
    {
        public static readonly float DefaultDepth = 1000;
        public static readonly float DefaultHeight = 720;
        public static readonly float DefaultWidth = 1280;

        public UIComponent()
        {
            Resolution = new Vector3(DefaultWidth, DefaultHeight, DefaultDepth);
            Size = new Vector3(DefaultWidth / 1000.0f, DefaultHeight / 1000.0f, 1.0f);
        }

        /// <summary>
        ///   Gets or sets the UI page.
        /// </summary>
        /// <userdoc>The UI page.</userdoc>
        [DataMember(10)]
        [Display("Page")]
        public UIPage Page { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the UI should be drawn fullscreen.
        /// </summary>
        /// <userdoc>Check this checkbox to display UI of this component on fullscreen. Uncheck it to display UI using standard camera.</userdoc>
        [DataMember(20)]
        [Display("Full Screen")]
        [DefaultValue(true)]
        public bool IsFullScreen { get; set; } = true;

        /// <summary>
        ///   Gets or sets the virtual resolution of the UI, in virtual pixels.
        /// </summary>
        /// <userdoc>The resolution of the UI in virtual pixels.</userdoc>
        [DataMember(30)]
        [Display("Resolution")]
        public Vector3 Resolution { get; set; }

        /// <summary>
        ///   Gets or sets the actual size of the UI component in world units. This value is ignored in fullscreen mode.
        /// </summary>
        /// <userdoc>The size of the UI component in world units when not displayed in fullscreen mode.</userdoc>
        [DataMember(35)]
        [Display("Size")]
        public Vector3 Size { get; set; }

        /// <summary>
        ///   Gets or sets a value to indicate how the virtual resolution value should be interpreted.
        /// </summary>
        /// <value>A value of <see cref="Rendering.ResolutionStretch"/> indicating how the virtual resolution value should be interpreted.</value>
        /// <userdoc>Indicates how the virtual resolution value should be interpreted.</userdoc>
        [DataMember(40)]
        [Display("Resolution Stretch")]
        [DefaultValue(ResolutionStretch.FixedWidthAdaptableHeight)]
        public ResolutionStretch ResolutionStretch { get; set; } = ResolutionStretch.FixedWidthAdaptableHeight;

        /// <summary>
        ///   Gets or sets a value indicating whether the UI should be displayed as a billboard.
        /// </summary>
        /// <value>
        ///   <c>true</c> to display the UI as a billboard, always rotated to face the camera;
        ///   <c>false</c> otherwise.
        /// </value>
        /// <userdoc>If checked, the UI is displayed as a billboard, automatically rotated to face the camera.</userdoc>
        [DataMember(50)]
        [Display("Billboard")]
        [DefaultValue(true)]
        public bool IsBillboard { get; set; } = true;

        /// <summary>
        ///   Gets or sets the value indicating of the UI texts should be snapped to the closest pixel.
        /// </summary>
        /// <value>
        ///   <c>true</c> to display the texts in the UI pixel perfect;
        ///   <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        ///   Pixel-perfect rendering of text is only effective when <see cref="IsFullScreen"/> or <see cref="IsFixedSize"/> is set,
        ///   as well as <see cref="IsBillboard"/>.
        /// </remarks>
        /// <userdoc>If checked, all the text of the UI is snapped to the closest pixel (pixel perfect). This is only effective if IsFullScreen or IsFixedSize is set, as well as IsBillboard.</userdoc>
        [DataMember(60)]
        [Display("Snap Text")]
        [DefaultValue(true)]
        public bool SnapText { get; set; } = true;

        /// <summary>
        ///   Gets or sets a value indicating whether the UI should have always a fixed size on the screen.
        /// </summary>
        /// <value>
        ///   <c>true</c> to display the UI with a fixed size on the screen;
        ///   <c>false</c> otherwise.
        /// </value>
        /// <userdoc>
        ///   Indicates whether the UI should be always a fixed size on the screen.
        ///   A fixed size component with a height of 1 unit will be 0.1 of the screen size.
        /// </userdoc>
        [DataMember(70)]
        [Display("Fixed Size")]
        [DefaultValue(false)]
        public bool IsFixedSize { get; set; } = false;

        /// <summary>
        ///   Gets or sets the render group the UI will be rendered to.
        /// </summary>
        [DataMember(80)]
        [Display("Render group")]
        [DefaultValue(RenderGroup.Group0)]
        public RenderGroup RenderGroup { get; set; }

        /// <summary>
        ///   A fixed size UI component with height of 1 will be this much of the vertical resolution on screen
        /// </summary>
        [DataMemberIgnore]
        public const float FixedSizeVerticalUnit = 1;   // 100% of the vertical resolution
    }
}
