// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Represents a <see cref="GameSystemBase"/> that allows to draw to another window or control. Currently only valid on desktop with Windows.Forms.
    /// </summary>
    public class GameWindowRenderer : GameSystemBase
    {
        private PixelFormat preferredBackBufferFormat;
        private int preferredBackBufferHeight;
        private int preferredBackBufferWidth;
        private PixelFormat preferredDepthStencilFormat;
        private bool isBackBufferToResize;
        private GraphicsPresenter savedPresenter;
        private bool beginDrawOk;
        private bool windowUserResized;

        /// <summary>
        ///   Initializes a new instance of the <see cref="GameWindowRenderer" /> class.
        /// </summary>
        /// <param name="registry">The registry of game services.</param>
        /// <param name="gameContext">The window context.</param>
        public GameWindowRenderer(IServiceRegistry registry, GameContext gameContext)
            : base(registry)
        {
            GameContext = gameContext;
        }

        /// <summary>
        ///   Gets the underlying context where to render.
        /// </summary>
        /// <value>The underlying <see cref="Games.GameContext"/>.</value>
        public GameContext GameContext { get; private set; }

        /// <summary>
        ///   Gets the window where to render.
        /// </summary>
        /// <value>The window.</value>
        public GameWindow Window { get; private set; }

        /// <summary>
        ///   Gets or sets the presenter.
        /// </summary>
        /// <value>The <see cref="GraphicsPresenter"/> in charge of presenting the buffers to the screen.</value>
        public GraphicsPresenter Presenter { get; protected set; }

        /// <summary>
        ///   Gets or sets the preferred backbuffer pixel format.
        /// </summary>
        /// <value>The preferred backbuffer format.</value>
        public PixelFormat PreferredBackBufferFormat
        {
            get => preferredBackBufferFormat;

            set
            {
                if (preferredBackBufferFormat != value)
                {
                    preferredBackBufferFormat = value;
                    isBackBufferToResize = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred height of the backbuffer, in pixels.
        /// </summary>
        /// <value>The preferred height of the backbuffer, in pixels.</value>
        public int PreferredBackBufferHeight
        {
            get => preferredBackBufferHeight;

            set
            {
                if (preferredBackBufferHeight != value)
                {
                    preferredBackBufferHeight = value;
                    isBackBufferToResize = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred width of the backbuffer, in pixels.
        /// </summary>
        /// <value>The preferred width of the backbuffer, in pixels.</value>
        public int PreferredBackBufferWidth
        {
            get => preferredBackBufferWidth;

            set
            {
                if (preferredBackBufferWidth != value)
                {
                    preferredBackBufferWidth = value;
                    isBackBufferToResize = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred depth-buffer and stencil format.
        /// </summary>
        /// <value>The preferred depth-buffer and stencil format.</value>
        public PixelFormat PreferredDepthStencilFormat
        {
            get => preferredDepthStencilFormat;
            set => preferredDepthStencilFormat = value;
        }

        public override void Initialize()
        {
            var gamePlatform = Services.GetService<IGamePlatform>();
            GameContext.RequestedWidth = PreferredBackBufferWidth;
            GameContext.RequestedHeight = PreferredBackBufferHeight;
            Window = gamePlatform.CreateWindow(GameContext);
            Window.Visible = true;

            Window.ClientSizeChanged += WindowOnClientSizeChanged;

            base.Initialize();
        }

        protected override void Destroy()
        {
            Presenter?.Dispose();
            Presenter = null;
            Window?.Dispose();
            Window = null;

            base.Destroy();
        }

        private Vector2 GetRequestedSize(out PixelFormat format)
        {
            var bounds = Window.ClientBounds;
            format = PreferredBackBufferFormat == PixelFormat.None ? PixelFormat.R8G8B8A8_UNorm : PreferredBackBufferFormat;
            return new Vector2(
                PreferredBackBufferWidth == 0 || windowUserResized ? bounds.Width : PreferredBackBufferWidth,
                PreferredBackBufferHeight == 0 || windowUserResized ? bounds.Height : PreferredBackBufferHeight);
        }

        protected virtual void CreateOrUpdatePresenter()
        {
            if (Presenter is null)
            {
                var size = GetRequestedSize(out PixelFormat resizeFormat);
                var presentationParameters = new PresentationParameters((int)size.X, (int)size.Y, Window.NativeWindow, resizeFormat);
                presentationParameters.DepthStencilFormat = PreferredDepthStencilFormat;
                presentationParameters.PresentationInterval = PresentInterval.Immediate;
                Presenter = new SwapChainGraphicsPresenter(GraphicsDevice, presentationParameters);

                isBackBufferToResize = false;
            }
        }

        public override bool BeginDraw()
        {
            if (GraphicsDevice != null && Window.Visible)
            {
                savedPresenter = GraphicsDevice.Presenter;

                CreateOrUpdatePresenter();

                if (isBackBufferToResize || windowUserResized)
                {
                    var size = GetRequestedSize(out PixelFormat resizeFormat);
                    Presenter.Resize((int) size.X, (int) size.Y, resizeFormat);

                    isBackBufferToResize = false;
                    windowUserResized = false;
                }

                GraphicsDevice.Presenter = Presenter;

                beginDrawOk = true;
                return true;
            }

            beginDrawOk = false;
            return false;
        }

        public override void EndDraw()
        {
            if (beginDrawOk && GraphicsDevice != null)
            {
                try
                {
                    Presenter.Present();
                }
                catch (GraphicsException ex)
                {
                    if (ex.Status != GraphicsDeviceStatus.Removed &&
                        ex.Status != GraphicsDeviceStatus.Reset)
                    {
                        throw;
                    }
                }

                if (savedPresenter != null)
                {
                    GraphicsDevice.Presenter = savedPresenter;
                }
            }
        }

        private void WindowOnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            windowUserResized = true;
        }
    }
}
