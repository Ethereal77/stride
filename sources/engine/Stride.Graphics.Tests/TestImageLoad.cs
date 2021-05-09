// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.IO;
using Stride.Core.Mathematics;
using Stride.Games;

using Xunit;

namespace Stride.Graphics.Tests
{
    public class TestImageLoad : GraphicTestGameBase
    {
        private SpriteBatch spriteBatch;
        private Texture jpg;
        private Texture png;

        public TestImageLoad() { }

        protected override void RegisterTests()
        {
            base.RegisterTests();

            FrameGameSystem.Draw(DrawImages).TakeScreenshot();
        }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            using (var pngStream = Content.FileProvider.OpenStream("PngImage", VirtualFileMode.Open, VirtualFileAccess.Read))
            using (var pngImage = Image.Load(pngStream, GraphicsDevice.ColorSpace == ColorSpace.Linear))
                png = Texture.New(GraphicsDevice, pngImage);

            using (var jpgStream = Content.FileProvider.OpenStream("JpegImage", VirtualFileMode.Open, VirtualFileAccess.Read))
            using (var jpgImage = Image.Load(jpgStream, GraphicsDevice.ColorSpace == ColorSpace.Linear))
                jpg = Texture.New(GraphicsDevice, jpgImage);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (!ScreenShotAutomationEnabled)
                DrawImages();
        }

        private void DrawImages()
        {
            GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.BackBuffer, Color.AntiqueWhite);
            GraphicsContext.CommandList.Clear(GraphicsDevice.Presenter.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer);
            GraphicsContext.CommandList.SetRenderTargetAndViewport(GraphicsDevice.Presenter.DepthStencilBuffer, GraphicsDevice.Presenter.BackBuffer);
            spriteBatch.Begin(GraphicsContext);

            var screenSize = new Vector2(GraphicsDevice.Presenter.BackBuffer.ViewWidth, GraphicsDevice.Presenter.BackBuffer.ViewHeight);

            spriteBatch.Draw(jpg, new Rectangle(0, 0, (int)screenSize.X, (int)(screenSize.Y / 2)), Color.White);
            spriteBatch.Draw(png, new Rectangle(0, (int)(screenSize.Y / 2), (int)screenSize.X, (int)(screenSize.Y / 2)), Color.White);

            spriteBatch.End();
        }

        /// <summary>
        /// Run the test
        /// </summary>
        [Fact]
        public void RunImageLoad()
        {
            RunGameTest(new TestImageLoad());
        }
    }
}
