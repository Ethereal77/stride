// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;

using Xunit;

using Xenko.Graphics;
using Xenko.Rendering.Sprites;
using Xenko.UI.Controls;

namespace Xenko.UI.Tests.Regression
{
    /// <summary>
    /// Class for rendering tests on the <see cref="Button"/>  that has <see cref="Button.SizeToContent"/> set to <c>false</c>.
    /// </summary>
    public class ImageButtonTest : UITestGameBase
    {
        private Button button;

        public ImageButtonTest()
        {
        }

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

        /// <summary>
        /// Launch the Image test.
        /// </summary>
        internal static void Main()
        {
            using (var game = new ImageButtonTest())
                game.Run();
        }
    }
}
