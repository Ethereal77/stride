// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Assets.Presentation.AssetEditors.UIEditor.Game;
using Stride.UI;
using Stride.UI.Controls;

namespace Stride.Assets.Presentation.AssetEditors.UIEditor.Adorners
{
    /// <summary>
    /// Represents an adorner with a <see cref="Border"/> visual.
    /// </summary>
    internal abstract class BorderAdorner : AdornerBase<Border>
    {
        private float borderThickness;
        private Vector3 size;

        protected BorderAdorner(UIEditorGameAdornerService service, UIElement gameSideElement)
            : base(service, gameSideElement)
        {
            Visual = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };

            InitializeAttachedProperties();
        }

        public Color BackgroundColor
        {
            get { return Visual.BackgroundColor; }
            set { Visual.BackgroundColor = value; }
        }

        public Color BorderColor
        {
            get { return Visual.BorderColor; }
            set { Visual.BorderColor = value; }
        }

        public float BorderThickness
        {
            get { return borderThickness; }
            set
            {
                borderThickness = value;
                UpdateBorderThickness();
                UpdateSize();
            }
        }

        public Vector3 Size
        {
            get { return size; }
            set
            {
                size = value;
                UpdateSize();
            }
        }

        public sealed override Border Visual { get; }

        protected virtual void UpdateBorderThickness()
        {
            Visual.BorderThickness = Thickness.UniformCuboid(borderThickness);
        }

        protected virtual void UpdateSize()
        {
            // border thickness is added to the total size
            Visual.Size = size + new Vector3(BorderThickness*2);
        }
    }
}
