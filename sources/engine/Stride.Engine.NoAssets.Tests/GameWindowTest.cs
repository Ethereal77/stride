// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Graphics.Regression;
using Stride.Games;

using Xunit;

namespace Stride.Engine.Tests
{
    public class GameWindowTest : GameTestBase
    {
        [Theory]
        [InlineData(AppContextType.Desktop)]
        public void RenderToWindow(AppContextType contextType)
        {
            PerformTest(game =>
            {
                var context = GameContextFactory.NewGameContext(contextType, isUserManagingRun: true);
                var windowRenderer = new GameWindowRenderer(game.Services, context)
                {
                    PreferredBackBufferWidth = 640,
                    PreferredBackBufferHeight = 480,
                };
                windowRenderer.Initialize();
                ((IContentable) windowRenderer).LoadContent();

                var messageLoop = windowRenderer.Window.CreateUserManagedMessageLoop();
                messageLoop.NextFrame();

                windowRenderer.BeginDraw();
                game.GraphicsContext.CommandList.Clear(windowRenderer.Presenter.BackBuffer, Color.Blue);
                windowRenderer.EndDraw();

                game.SaveImage(windowRenderer.Presenter.BackBuffer, "Clear");

                windowRenderer.Dispose();
            });
        }
    }
}
