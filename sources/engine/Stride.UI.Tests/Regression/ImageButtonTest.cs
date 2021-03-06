// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Graphics;
using Stride.Rendering.Sprites;
using Stride.UI.Controls;

using Xunit;

namespace Stride.UI.Tests.Regression
{
    /// <summary>
    ///   Test class for rendering tests on the <see cref="Button"/>  that has <see cref="Button.SizeToContent"/> set to <c>false</c>.
    /// </summary>
    public class ImageButtonTest : UITestGameBase
    {
        private Button button;

        public ImageButtonTest() { }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            button = new Button
            {
                PressedImage = (SpriteFromTexture)new Sprite(Content.Load<Texture>("ImageButtonPressed")),
                NotPressedImage = (SpriteFromTexture)new Sprite(Content.Load<Texture>("ImageButtonNotPressed")),
                Content = null,
                SizeToContent = false
            };

            UIComponent.Page = new Engine.UIPage { RootElement = button };
        }

        protected override void RegisterTests()
        {
            base.RegisterTests();
            FrameGameSystem.DrawOrder = -1;
            FrameGameSystem.TakeScreenshot();
            FrameGameSystem.Draw(DrawTest1).TakeScreenshot();
        }

        private void DrawTest1()
        {
            button.RaiseTouchDownEvent(new TouchEventArgs());
        }

        [Fact]
        public void RunImageButtonTest()
        {
            RunGameTest(new ImageButtonTest());
        }
    }
}
