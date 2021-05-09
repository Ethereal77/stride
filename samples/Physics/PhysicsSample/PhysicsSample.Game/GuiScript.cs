// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace PhysicsSample
{
    /// <summary>
    /// The script in charge of displaying the UI
    /// </summary>
    public class GuiScript : StartupScript
    {
        public SpriteFont Font;

        public override void Start()
        {
            base.Start();

            var textBlock = new TextBlock
            {
                Text = "Shoot the cubes!",
                Font = Font,
                TextColor = Color.White,
                TextSize = 60
            };
            textBlock.SetCanvasPinOrigin(new Vector3(0.5f, 0.5f, 0));
            textBlock.SetCanvasRelativePosition(new Vector3(0.5f, 0.9f, 0f));

            Entity.Get<UIComponent>().Page = new UIPage { RootElement = new Canvas { Children = { textBlock } } };
        }
    }
}
