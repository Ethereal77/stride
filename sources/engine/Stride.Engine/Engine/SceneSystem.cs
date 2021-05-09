// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Core.Serialization.Contents;
using Stride.Games;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents the <see cref="GameSystem"/> that handles the <see cref="Scene"/>s of a game.
    /// </summary>
    public class SceneSystem : GameSystemBase
    {
        private static readonly Logger Log = GlobalLogger.GetLogger("SceneSystem");

        private RenderContext renderContext;
        private RenderDrawContext renderDrawContext;

        private int previousWidth;
        private int previousHeight;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SceneSystem" /> class.
        /// </summary>
        /// <param name="registry">The registry of game services.</param>
        public SceneSystem(IServiceRegistry registry)
            : base(registry)
        {
            Enabled = true;
            Visible = true;

            GraphicsCompositor = new GraphicsCompositor();
        }

        /// <summary>
        ///   Gets or sets the root scene.
        /// </summary>
        /// <value>The scene</value>
        public SceneInstance SceneInstance { get; set; }

        /// <summary>
        ///   Gets or sets the URL of the <see cref="Scene"/> to load at initialization.
        /// </summary>
        public string InitialSceneUrl { get; set; }

        /// <summary>
        ///   Gets or sets the URL of the <see cref="Rendering.Compositing.GraphicsCompositor"/> to load at initialization.
        /// </summary>
        public string InitialGraphicsCompositorUrl { get; set; }

        /// <summary>
        ///   Gets or sets the URL of the <see cref="Texture"/> to show as a splash screen and load at initialization.
        /// </summary>
        public string SplashScreenUrl { get; set; }
        /// <summary>
        ///   Gets or sets the color to use as background for the splash screen.
        /// </summary>
        public Color4 SplashScreenColor { get; set; }
        /// <summary>
        ///   Gets or sets a value indicating whether to show a splash screen.
        /// </summary>
        /// <remarks>
        ///   If the splash screen rendering is enabled, it will only be shown if a splash screen texture (<see cref="SplashScreenUrl"/>) is present,
        ///   and only in release builds.
        /// </remarks>
        public bool SplashScreenEnabled { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref="Rendering.Compositing.GraphicsCompositor"/> used to render the scenes.
        /// </summary>
        public GraphicsCompositor GraphicsCompositor { get; set; }

        private Task<Scene> sceneTask;
        private Task<GraphicsCompositor> compositorTask;

        private const double MinSplashScreenTime = 4.0f;
        private const float SplashScreenFadeTime = 1.0f;

        private double fadeTime;
        private Texture splashScreenTexture;

        public enum SplashScreenState
        {
            Invalid,
            Intro,
            FadingIn,
            Showing,
            FadingOut,
        }

        private SplashScreenState splashScreenState = SplashScreenState.Invalid;

        protected override void LoadContent()
        {
            var content = Services.GetSafeServiceAs<ContentManager>();
            var graphicsContext = Services.GetSafeServiceAs<GraphicsContext>();

            if (SplashScreenUrl != null && content.Exists(SplashScreenUrl))
            {
                splashScreenTexture = content.Load<Texture>(SplashScreenUrl, ContentManagerLoaderSettings.StreamingDisabled);
                splashScreenState = splashScreenTexture != null ? SplashScreenState.Intro : SplashScreenState.Invalid;
                SplashScreenEnabled = true;
            }

            // Preload the scene if it exists and show splash screen
            if (InitialSceneUrl != null && content.Exists(InitialSceneUrl))
            {
                if (SplashScreenEnabled)
                    sceneTask = content.LoadAsync<Scene>(InitialSceneUrl);
                else
                    SceneInstance = new SceneInstance(Services, content.Load<Scene>(InitialSceneUrl));
            }

            if (InitialGraphicsCompositorUrl != null && content.Exists(InitialGraphicsCompositorUrl))
            {
                if (SplashScreenEnabled)
                    compositorTask = content.LoadAsync<GraphicsCompositor>(InitialGraphicsCompositorUrl);
                else
                    GraphicsCompositor = content.Load<GraphicsCompositor>(InitialGraphicsCompositorUrl);
            }

            // Create the drawing context
            renderContext = RenderContext.GetShared(Services);
            renderDrawContext = new RenderDrawContext(Services, renderContext, graphicsContext);
        }

        protected override void Destroy()
        {
            if (SceneInstance != null)
            {
                ((IReferencable) SceneInstance).Release();
                SceneInstance = null;
            }

            if (GraphicsCompositor != null)
            {
                GraphicsCompositor.Dispose();
                GraphicsCompositor = null;
            }

            base.Destroy();
        }

        public override void Update(GameTime gameTime)
        {
            // Execute Update step of SceneInstance
            // This will run entity processors
            SceneInstance?.Update(gameTime);
        }

        private void RenderSplashScreen(Color4 color, BlendStateDescription blendState)
        {
            var renderTarget = Game.GraphicsContext.CommandList.RenderTarget;
            Game.GraphicsContext.CommandList.Clear(renderTarget, SplashScreenColor);

            var viewWidth = renderTarget.Width;
            var viewHeight = renderTarget.Height;
            var viewportSize = Math.Min(viewWidth, viewHeight);

            var initialViewport = Game.GraphicsContext.CommandList.Viewport;

            var x = (viewWidth - viewportSize) / 2;
            var y = (viewHeight - viewportSize) / 2;
            var newViewport = new Viewport(x, y, viewportSize, viewportSize);

            Game.GraphicsContext.CommandList.SetViewport(newViewport);
            Game.GraphicsContext.DrawTexture(splashScreenTexture, color, blendState);

            Game.GraphicsContext.CommandList.SetViewport(initialViewport);
        }

        public override void Draw(GameTime gameTime)
        {
            // Reset the context
            renderContext.Reset();

            var renderTarget = renderDrawContext.CommandList.RenderTarget;

            // If the width or height changed, we have to recycle all temporary allocated resources.
            // NOTE: We assume that they are mostly resolution dependent.
            if (previousWidth != renderTarget.ViewWidth ||
                previousHeight != renderTarget.ViewHeight)
            {
                // Force a recycle of all allocated temporary textures
                renderContext.Allocator.Recycle(link => true);
            }

            previousWidth = renderTarget.ViewWidth;
            previousHeight = renderTarget.ViewHeight;

            // Update the entities at draw time.
            renderContext.Time = gameTime;

            // The camera processor needs the graphics compositor
            using (renderContext.PushTagAndRestore(GraphicsCompositor.Current, GraphicsCompositor))
            {
                // Execute Draw step of SceneInstance
                // This will run entity processors
                SceneInstance?.Draw(renderContext);
            }

            // Render phase
            // TODO: GRAPHICS REFACTOR
            //context.GraphicsDevice.Parameters.Set(GlobalKeys.Time, (float)gameTime.Total.TotalSeconds);
            //context.GraphicsDevice.Parameters.Set(GlobalKeys.TimeStep, (float)gameTime.Elapsed.TotalSeconds);

            renderDrawContext.ResourceGroupAllocator.Flush();
            renderDrawContext.QueryManager.Flush();

            // Push context (pop after using)
            using (renderDrawContext.RenderContext.PushTagAndRestore(SceneInstance.Current, SceneInstance))
            {
                GraphicsCompositor?.Draw(renderDrawContext);
            }

            // Do this here, make sure Graphics Compositor and Scene are updated/rendered the next frame!
            if (sceneTask != null && compositorTask != null)
            {
                switch (splashScreenState)
                {
                    case SplashScreenState.Invalid:
                            if (sceneTask.IsCompleted && compositorTask.IsCompleted)
                            {
                                SceneInstance = new SceneInstance(Services, sceneTask.Result);
                                GraphicsCompositor = compositorTask.Result;
                                sceneTask = null;
                                compositorTask = null;
                            }
                            break;

                    case SplashScreenState.Intro:
                            Game.GraphicsContext.CommandList.Clear(Game.GraphicsContext.CommandList.RenderTarget, SplashScreenColor);

                            if (gameTime.Total.TotalSeconds > SplashScreenFadeTime)
                            {
                                splashScreenState = SplashScreenState.FadingIn;
                                fadeTime = 0.0f;
                            }
                            break;

                    case SplashScreenState.FadingIn:
                        {
                            var color = Color4.White;
                            var factor = MathUtil.SmoothStep((float)fadeTime / SplashScreenFadeTime);
                            color *= factor;
                            if (factor >= 1.0f)
                            {
                                splashScreenState = SplashScreenState.Showing;
                            }

                            fadeTime += gameTime.Elapsed.TotalSeconds;

                            RenderSplashScreen(color, BlendStates.AlphaBlend);
                            break;
                        }

                    case SplashScreenState.Showing:
                            RenderSplashScreen(Color4.White, BlendStates.Default);

                                if (gameTime.Total.TotalSeconds > MinSplashScreenTime &&
                                    sceneTask.IsCompleted && compositorTask.IsCompleted)
                            {
                                splashScreenState = SplashScreenState.FadingOut;
                                fadeTime = 0.0f;
                            }
                            break;

                    case SplashScreenState.FadingOut:
                        {
                            var color = Color4.White;
                            var factor = (MathUtil.SmoothStep((float)fadeTime / SplashScreenFadeTime) * -1) + 1;
                            color *= factor;
                            if (factor <= 0.0f)
                            {
                                splashScreenState = SplashScreenState.Invalid;
                            }

                            fadeTime += gameTime.Elapsed.TotalSeconds;

                            RenderSplashScreen(color, BlendStates.AlphaBlend);
                            break;
                        }
                }
            }
        }
    }
}
