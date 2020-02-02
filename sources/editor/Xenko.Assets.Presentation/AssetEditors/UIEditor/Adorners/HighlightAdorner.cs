// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Runtime.CompilerServices;

using Xenko.Core.Mathematics;
using Xenko.Assets.Presentation.AssetEditors.UIEditor.Game;
using Xenko.UI;

namespace Xenko.Assets.Presentation.AssetEditors.UIEditor.Adorners
{
    /// <summary>
    /// Represents an adorner that is highlighted on mouse over.
    /// </summary>
    internal sealed class HighlightAdorner : BorderAdorner
    {
        public HighlightAdorner(UIEditorGameAdornerService service, UIElement gameSideElement)
            : base(service, gameSideElement)
        {
            BackgroundColor = Color.Transparent;
            Visual.Name = "[Hightlight]";
            Visual.CanBeHitByUser = true;
            // Hidden by default
            Visual.Opacity = 0.0f;
        }

        public bool IsHighlighted => Visual.Opacity > 0.0f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Highlight()
        {
            Visual.Opacity = 1.0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unlit()
        {
            Visual.Opacity = 0.0f;
        }

        public override void Update(Vector3 position)
        {
            UpdateFromSettings();
            Size = GameSideElement.RenderSize;
        }

        protected override void UpdateSize()
        {
            base.UpdateSize();
            Visual.Margin = Thickness.UniformCuboid(-BorderThickness);
        }

        private void UpdateFromSettings()
        {
            var editor = Service.Controller.Editor;

            BorderColor = editor.HighlightColor;
            BorderThickness = editor.HighlightThickness;
        }
    }
}
