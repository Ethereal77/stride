// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Rendering.Sprites;
using Stride.UI.Controls;

using Xunit;

namespace Stride.UI.Tests.Regression
{
    /// <summary>
    ///   Test class for rendering tests on the <see cref="ImageElement"/> .
    /// </summary>
    public class ImageTest : UITestGameBase
    {
        private ImageElement imageElement;

        public ImageTest() { }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            imageElement = new ImageElement { Source = (SpriteFromTexture)new Sprite(Content.Load<Texture>("uv"))};
            UIComponent.Page = new Engine.UIPage { RootElement = imageElement };
        }

        protected override void RegisterTests()
        {
            base.RegisterTests();

            FrameGameSystem.TakeScreenshot();
            FrameGameSystem.Draw(() => ChangeImageColor(Color.Brown)).TakeScreenshot();
            FrameGameSystem.Draw(() => ChangeImageColor(Color.Blue)).TakeScreenshot();
            FrameGameSystem.Draw(() => ChangeImageColor(Color.Red)).TakeScreenshot();
            FrameGameSystem.Draw(() => ChangeImageColor(Color.Lime)).TakeScreenshot();
        }

        private void ChangeImageColor(Color color)
        {
            imageElement.Color = color;
        }

        [Fact]
        public void RunImageTest()
        {
            RunGameTest(new ImageTest());
        }
    }
}
