// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Graphics.Font;
using Stride.UI.Controls;

namespace Stride.UI.Renderers
{
    /// <summary>
    /// The default renderer for <see cref="TextBlock"/>.
    /// </summary>
    internal class DefaultTextBlockRenderer : ElementRenderer
    {
        public DefaultTextBlockRenderer(IServiceRegistry services)
            : base(services)
        {
        }

        public override void RenderColor(UIElement element, UIRenderingContext context)
        {
            base.RenderColor(element, context);

            var textBlock = (TextBlock)element;

            if (textBlock.Font == null || textBlock.TextToDisplay == null)
                return;
            
            var drawCommand = new SpriteFont.InternalUIDrawCommand
            {
                Color = textBlock.RenderOpacity * textBlock.TextColor,
                DepthBias = context.DepthBias,
                RealVirtualResolutionRatio = element.LayoutingContext.RealVirtualResolutionRatio,
                RequestedFontSize = textBlock.ActualTextSize,
                Batch = Batch,
                SnapText = context.ShouldSnapText && !textBlock.DoNotSnapText,
                Matrix = textBlock.WorldMatrixInternal,
                Alignment = textBlock.TextAlignment,
                TextBoxSize = new Vector2(textBlock.ActualWidth, textBlock.ActualHeight)
            };

            if (textBlock.Font.FontType == SpriteFontType.SDF)
            {
                Batch.End();

                Batch.BeginCustom(context.GraphicsContext, 1);                
            }

            Batch.DrawString(textBlock.Font, textBlock.TextToDisplay, ref drawCommand);

            if (textBlock.Font.FontType == SpriteFontType.SDF)
            {
                Batch.End();

                Batch.BeginCustom(context.GraphicsContext, 0);
            }
        }
    }
}
