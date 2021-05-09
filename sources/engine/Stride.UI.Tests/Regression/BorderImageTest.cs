// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Rendering.Sprites;
using Stride.UI.Controls;
using Stride.UI.Panels;

using Xunit;

namespace Stride.UI.Tests.Regression
{
    /// <summary>
    ///   Class for rendering tests for <see cref="ImageElement"/>.
    /// </summary>
    public class BorderImageTest : UITestGameBase
    {
        private StackPanel stackPanel;

        public BorderImageTest() { }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            var sprite = (SpriteFromTexture) new Sprite(Content.Load<Texture>("BorderButton")) { Borders = new Vector4(64, 64, 64, 64) };

            var bi1 = new ImageElement { Source = sprite, Height = 150 };
            var bi2 = new ImageElement { Source = sprite, Height = 300 };
            var bi3 = new ImageElement { Source = sprite, Height = 500 };

            stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            stackPanel.Children.Add(bi1);
            stackPanel.Children.Add(bi2);
            stackPanel.Children.Add(bi3);

            UIComponent.Page = new Engine.UIPage { RootElement = new ScrollViewer { Content = stackPanel, ScrollMode = ScrollingMode.HorizontalVertical } };
        }

        protected override void RegisterTests()
        {
            base.RegisterTests();

            FrameGameSystem.DrawOrder = -1;
            FrameGameSystem.Draw(DrawSmallerElement).TakeScreenshot();
            FrameGameSystem.Draw(DrawRealSizeElement).TakeScreenshot();
            FrameGameSystem.Draw(DrawBiggerElement).TakeScreenshot();
        }

        private void DrawSmallerElement()
        {
            stackPanel?.ScrolllToElement(0);
        }

        private void DrawRealSizeElement()
        {
            stackPanel?.ScrolllToElement(1);
        }

        private void DrawBiggerElement()
        {
            stackPanel?.ScrolllToElement(2);
        }

        [Fact]
        public void RunBorderImageTest()
        {
            RunGameTest(new BorderImageTest());
        }
    }
}
