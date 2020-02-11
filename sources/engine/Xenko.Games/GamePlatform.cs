// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type

using System;
using System.Collections.Generic;

using Xenko.Core;
using Xenko.Graphics;

namespace Xenko.Games
{
    internal abstract class GamePlatform : ReferenceBase, IGraphicsDeviceFactory, IGamePlatform
    {
        private bool hasExitRan = false;

        protected readonly GameBase game;

        protected readonly IServiceRegistry Services;

        protected GameWindow gameWindow;

        public string FullName { get; protected set; } = string.Empty;

        protected GamePlatform(GameBase game)
        {
            this.game = game;
            Services = game.Services;
        }

        public static GamePlatform Create(GameBase game)
        {
            // Here we cover all Desktop variants: SDL, WinForms,...
            return new GamePlatformWindows(game);
        }

        public abstract string DefaultAppDirectory { get; }

        public object WindowContext { get; set; }

        public event EventHandler<EventArgs> Activated;

        public event EventHandler<EventArgs> Deactivated;

        public event EventHandler<EventArgs> Exiting;

        public event EventHandler<EventArgs> Idle;

        public event EventHandler<EventArgs> Resume;

        public event EventHandler<EventArgs> Suspend;

        public event EventHandler<EventArgs> WindowCreated;

        public GameWindow MainWindow
        {
            get
            {
                return gameWindow;
            }
        }

        internal abstract GameWindow GetSupportedGameWindow(AppContextType type);

        public virtual GameWindow CreateWindow(GameContext gameContext)
        {
            var window = GetSupportedGameWindow(gameContext.ContextType);
            if (window != null)
            {
                window.Services = Services;
                window.Initialize(gameContext);
                return window;
            }

            throw new ArgumentException("Game Window context not supported on this platform");
        }

        /// <summary>
        /// If <c>true</c>, <see cref="Game.Run()"/> is blocking until the game is exited, i.e. internal main loop is used.
        /// If <c>false</c>, <see cref="Game.Run()"/> returns immediately and the caller has to manage the main loop by invoking the <see cref="GameWindow.RunCallback"/>.
        /// </summary>
        public bool IsBlockingRun { get; protected set; }

        public void Run(GameContext gameContext)
        {
            IsBlockingRun = !gameContext.IsUserManagingRun;

            gameWindow = CreateWindow(gameContext);

            // Register on Activated
            gameWindow.Activated += OnActivated;
            gameWindow.Deactivated += OnDeactivated;
            gameWindow.InitCallback = OnInitCallback;
            gameWindow.RunCallback = OnRunCallback;

            WindowCreated?.Invoke(this, EventArgs.Empty);

            gameWindow.Run();
        }

        private void OnRunCallback()
        {
            // If/else outside of try-catch to separate user-unhandled exceptions properly
            var unhandledException = game.UnhandledExceptionInternal;
            if (unhandledException != null)
            {
                // Catch exceptions and transmit them to UnhandledException event
                try
                {
                    Tick();
                }
                catch (Exception e)
                {
                    // Some system was listening for exceptions
                    unhandledException(this, new GameUnhandledExceptionEventArgs(e, false));
                    game.Exit();
                }
            }
            else
            {
                Tick();
            }
        }

        private void OnInitCallback()
        {
            // If/else outside of try-catch to separate user-unhandled exceptions properly
            var unhandledException = game.UnhandledExceptionInternal;
            if (unhandledException != null)
            {
                // Catch exceptions and transmit them to UnhandledException event
                try
                {
                    game.InitializeBeforeRun();
                }
                catch (Exception e)
                {
                    // Some system was listening for exceptions
                    unhandledException(this, new GameUnhandledExceptionEventArgs(e, false));
                    game.Exit();
                }
            }
            else
            {
                game.InitializeBeforeRun();
            }
        }

        private void Tick()
        {
            game.Tick();

            if (!IsBlockingRun && game.IsExiting() && !hasExitRan)
            {
                hasExitRan = true;
                OnExiting(this, EventArgs.Empty);
            }
        }

        public virtual void Exit()
        {
            // Notifies that the GameWindow should exit.
            gameWindow.Exiting = true;
        }

        protected void OnActivated(object source, EventArgs e)
        {
            Activated?.Invoke(this, e);
        }

        protected void OnDeactivated(object source, EventArgs e)
        {
            Deactivated?.Invoke(this, e);
        }

        protected void OnExiting(object source, EventArgs e)
        {
            Exiting?.Invoke(this, e);
        }

        protected void OnIdle(object source, EventArgs e)
        {
            Idle?.Invoke(this, e);
        }

        protected void OnResume(object source, EventArgs e)
        {
            Resume?.Invoke(this, e);
        }

        protected void OnSuspend(object source, EventArgs e)
        {
            Suspend?.Invoke(this, e);
        }

        protected void AddDevice(DisplayMode mode,  GraphicsDeviceInformation deviceBaseInfo, GameGraphicsParameters preferredParameters, List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            // TODO: Temporary woraround
            if (mode == null)
                mode = new DisplayMode(PixelFormat.R8G8B8A8_UNorm, 800, 480, new Rational(60, 1));

            var deviceInfo = deviceBaseInfo.Clone();

            deviceInfo.PresentationParameters.RefreshRate = mode.RefreshRate;

            if (preferredParameters.IsFullScreen)
            {
                deviceInfo.PresentationParameters.BackBufferFormat = mode.Format;
                deviceInfo.PresentationParameters.BackBufferWidth = mode.Width;
                deviceInfo.PresentationParameters.BackBufferHeight = mode.Height;
            }
            else
            {
                deviceInfo.PresentationParameters.BackBufferFormat = preferredParameters.PreferredBackBufferFormat;
                deviceInfo.PresentationParameters.BackBufferWidth = preferredParameters.PreferredBackBufferWidth;
                deviceInfo.PresentationParameters.BackBufferHeight = preferredParameters.PreferredBackBufferHeight;
            }

            deviceInfo.PresentationParameters.DepthStencilFormat = preferredParameters.PreferredDepthStencilFormat;
            deviceInfo.PresentationParameters.MultisampleCount = preferredParameters.PreferredMultisampleCount;

            if (!graphicsDeviceInfos.Contains(deviceInfo))
            {
                graphicsDeviceInfos.Add(deviceInfo);
            }
        }

        public virtual List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters preferredParameters)
        {
            var graphicsDeviceInfos = new List<GraphicsDeviceInformation>();

            // Iterate on each adapter
            foreach (var graphicsAdapter in GraphicsAdapterFactory.Adapters)
            {
                if (!string.IsNullOrEmpty(preferredParameters.RequiredAdapterUid) && graphicsAdapter.AdapterUid != preferredParameters.RequiredAdapterUid) continue;

                // Skip adapeters that don't have graphics output but only if no RequiredAdapterUid is provided
                if (graphicsAdapter.Outputs.Length == 0 && string.IsNullOrEmpty(preferredParameters.RequiredAdapterUid))
                {
                    continue;
                }

                var preferredGraphicsProfiles = preferredParameters.PreferredGraphicsProfile;

                // Iterate on each preferred graphics profile
                foreach (var featureLevel in preferredGraphicsProfiles)
                {
                    // Check if this profile is supported.
                    if (graphicsAdapter.IsProfileSupported(featureLevel))
                    {
                        var deviceInfo = new GraphicsDeviceInformation
                        {
                            Adapter = graphicsAdapter,
                            GraphicsProfile = featureLevel,
                            PresentationParameters =
                            {
                                MultisampleCount = preferredParameters.PreferredMultisampleCount,
                                IsFullScreen = preferredParameters.IsFullScreen,
                                PreferredFullScreenOutputIndex = preferredParameters.PreferredFullScreenOutputIndex,
                                PresentationInterval = preferredParameters.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate,
                                DeviceWindowHandle = MainWindow.NativeWindow,
                                ColorSpace = preferredParameters.ColorSpace,
                            },
                        };

                        var preferredMode = new DisplayMode(preferredParameters.PreferredBackBufferFormat,
                            preferredParameters.PreferredBackBufferWidth,
                            preferredParameters.PreferredBackBufferHeight,
                            preferredParameters.PreferredRefreshRate);

                        // if we want to switch to fullscreen, try to find only needed output, otherwise check them all
                        if (preferredParameters.IsFullScreen)
                        {
                            if (preferredParameters.PreferredFullScreenOutputIndex < graphicsAdapter.Outputs.Length)
                            {
                                var output = graphicsAdapter.Outputs[preferredParameters.PreferredFullScreenOutputIndex];
                                var displayMode = output.FindClosestMatchingDisplayMode(preferredGraphicsProfiles, preferredMode);
                                AddDevice(displayMode, deviceInfo, preferredParameters, graphicsDeviceInfos);
                            }
                        }
                        else
                        {
                            AddDevice(preferredMode, deviceInfo, preferredParameters, graphicsDeviceInfos);
                        }

                        // If the profile is supported, we are just using the first best one
                        break;
                    }
                }
            }

            return graphicsDeviceInfos;
        }

        public virtual GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var graphicsDevice = GraphicsDevice.New(deviceInformation.Adapter, deviceInformation.DeviceCreationFlags, gameWindow.NativeWindow, deviceInformation.GraphicsProfile);
            graphicsDevice.ColorSpace = deviceInformation.PresentationParameters.ColorSpace;
            graphicsDevice.Presenter = new SwapChainGraphicsPresenter(graphicsDevice, deviceInformation.PresentationParameters);

            return graphicsDevice;
        }

        public virtual void RecreateDevice(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation)
        {
            currentDevice.ColorSpace = deviceInformation.PresentationParameters.ColorSpace;
            currentDevice.Recreate(deviceInformation.Adapter ?? GraphicsAdapterFactory.Default, new[] { deviceInformation.GraphicsProfile }, deviceInformation.DeviceCreationFlags, gameWindow.NativeWindow);
        }

        public virtual void DeviceChanged(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation)
        {
            // Force to resize the gameWindow
            gameWindow.Resize(deviceInformation.PresentationParameters.BackBufferWidth, deviceInformation.PresentationParameters.BackBufferHeight);
        }

        public virtual GraphicsDevice ChangeOrCreateDevice(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation)
        {
            if (currentDevice == null)
            {
                currentDevice = CreateDevice(deviceInformation);
            }
            else
            {
                RecreateDevice(currentDevice, deviceInformation);
            }

            DeviceChanged(currentDevice, deviceInformation);

            return currentDevice;
        }

        protected override void Destroy()
        {
            if (gameWindow != null)
            {
                gameWindow.Dispose();
                gameWindow = null;
            }

            Activated = null;
            Deactivated = null;
            Exiting = null;
            Idle = null;
            Resume = null;
            Suspend = null;
        }
    }

    internal abstract class GamePlatform<TK> : GamePlatform
    {
        protected GamePlatform(GameBase game) : base(game)
        {
        }
    }
}
