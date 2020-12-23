// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Represents a class that manages the lifecycle of a <see cref="Graphics.GraphicsDevice"/>.
    /// </summary>
    public class GraphicsDeviceManager : ComponentBase, IGraphicsDeviceManager, IGraphicsDeviceService
    {
        /// <summary>
        ///   Default width for the back buffer.
        /// </summary>
        public static readonly int DefaultBackBufferWidth = 1280;
        /// <summary>
        ///   Default height for the back buffer.
        /// </summary>
        public static readonly int DefaultBackBufferHeight = 720;

        private readonly object lockDeviceCreation;

        private GameBase game;

        private bool deviceSettingsChanged;

        private bool isFullScreen;

        private MultisampleCount preferredMultisampleCount;
        private PixelFormat preferredBackBufferFormat;
        private int preferredBackBufferHeight;
        private int preferredBackBufferWidth;
        private Rational preferredRefreshRate;
        private PixelFormat preferredDepthStencilFormat;

        private DisplayOrientation supportedOrientations;

        private bool synchronizeWithVerticalRetrace;

        private int preferredFullScreenOutputIndex;

        private bool isChangingDevice;

        private int resizedBackBufferWidth;
        private int resizedBackBufferHeight;

        private bool isBackBufferToResize = false;

        private DisplayOrientation currentWindowOrientation;

        private bool beginDrawOk;

        private IGraphicsDeviceFactory graphicsDeviceFactory;

        private bool isReallyFullScreen;

        private ColorSpace preferredColorSpace;


        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsDeviceManager"/> class.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <exception cref="ArgumentNullException"><paramref name="game"/> cannot be a <c>null</c> reference.</exception>
        internal GraphicsDeviceManager(GameBase game)
        {
            this.game = game;
            if (this.game is null)
                throw new ArgumentNullException(nameof(game));

            lockDeviceCreation = new object();

            // Defines all default values
            SynchronizeWithVerticalRetrace = true;
            PreferredColorSpace = ColorSpace.Linear;
            PreferredBackBufferFormat = PixelFormat.R8G8B8A8_UNorm;
            PreferredDepthStencilFormat = PixelFormat.D24_UNorm_S8_UInt;
            preferredBackBufferWidth = DefaultBackBufferWidth;
            preferredBackBufferHeight = DefaultBackBufferHeight;
            preferredRefreshRate = new Rational(60, 1);
            PreferredMultisampleCount = MultisampleCount.None;
            PreferredGraphicsProfile = new[]
                {
                    GraphicsProfile.Level_11_2,
                    GraphicsProfile.Level_11_1,
                    GraphicsProfile.Level_11_0
                };

            graphicsDeviceFactory = game.Services.GetService<IGraphicsDeviceFactory>();
            if (graphicsDeviceFactory is null)
                throw new InvalidOperationException("IGraphicsDeviceFactory is not registered as a service");

            game.WindowCreated += GameOnWindowCreated;
        }

        private void GameOnWindowCreated(object sender, EventArgs eventArgs)
        {
            game.Window.ClientSizeChanged += Window_ClientSizeChanged;
            game.Window.OrientationChanged += Window_OrientationChanged;
            game.Window.FullscreenChanged += Window_FullscreenChanged;
        }


        #region Public Events

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

        #endregion

        public GraphicsDevice GraphicsDevice { get; internal set; }

        /// <summary>
        ///   Gets or sets the list of graphics profiles to select from the best feature to the lower feature.
        /// </summary>
        /// <value>The graphics profiles in order of preference, from most preferred to least preferred.</value>
        /// <remarks>
        ///   By default, the preferred graphics profile is set to <see cref="GraphicsProfile.Level_11_2"/> &gt;
        ///   <see cref="GraphicsProfile.Level_11_1"/> &gt; <see cref="GraphicsProfile.Level_11_0"/>.
        /// </remarks>
        public GraphicsProfile[] PreferredGraphicsProfile { get; set; }

        /// <summary>
        ///   Gets or sets the shader graphics profile that will be used to compile shaders.
        /// </summary>
        /// <value>The shader graphics profile.</value>
        /// <remarks>
        ///   If this property is not set, the profile used to compile the shader will be taken from the <see cref="GraphicsDevice"/>
        ///   based on the list provided by <see cref="PreferredGraphicsProfile"/>.
        /// </remarks>
        public GraphicsProfile? ShaderProfile { get; set; }

        /// <summary>
        ///   Gets or sets the device creation flags that will be used to create the <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <value>The device creation flags.</value>
        public DeviceCreationFlags DeviceCreationFlags { get; set; }

        /// <summary>
        ///   Gets or sets a string representing the unique Id of an adapter the engine will try to initialize.
        /// </summary>
        /// <value>
        ///   Unique Id of the adapter the engine will try to initialize; or <c>null</c>.
        /// </value>
        public string RequiredAdapterUid { get; set; }

        /// <summary>
        ///   Gets or sets the default color space.
        /// </summary>
        /// <value>The default color space.</value>
        public ColorSpace PreferredColorSpace
        {
            get => preferredColorSpace;

            set
            {
                if (preferredColorSpace != value)
                {
                    preferredColorSpace = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Sets the preferred graphics profiles.
        /// </summary>
        /// <param name="levels">The graphics profiles.</param>
        /// <seealso cref="PreferredGraphicsProfile"/>
        public void SetPreferredGraphicsProfile(params GraphicsProfile[] levels)
        {
            PreferredGraphicsProfile = levels;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to render in full screen.
        /// </summary>
        /// <value><c>true</c> to render in full screen; otherwise, <c>false</c>.</value>
        public bool IsFullScreen
        {
            get => isFullScreen;

            set
            {
                if (isFullScreen == value)
                    return;

                isFullScreen = value;
                deviceSettingsChanged = true;
            }
        }

        /// <summary>
        /// Gets or sets the preferred sample count for Multi-Sample Anti-Aliasing (MSAA).
        /// </summary>
        /// <value>
        ///   A value of <see cref="MultisampleCount"/> defining the number of samples. Specify
        ///   <see cref="MultisampleCount.None"/> to disable multi-sampling.
        /// </value>
        public MultisampleCount PreferredMultisampleCount
        {
            get => preferredMultisampleCount;

            set
            {
                if (preferredMultisampleCount != value)
                {
                    preferredMultisampleCount = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred back buffer format.
        /// </summary>
        /// <value>The preferred back buffer format.</value>
        public PixelFormat PreferredBackBufferFormat
        {
            get => preferredBackBufferFormat;

            set
            {
                if (preferredBackBufferFormat != value)
                {
                    preferredBackBufferFormat = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred height of the back buffer.
        /// </summary>
        /// <value>The preferred height of the back buffer.</value>
        public int PreferredBackBufferHeight
        {
            get => preferredBackBufferHeight;

            set
            {
                if (preferredBackBufferHeight != value)
                {
                    preferredBackBufferHeight = value;
                    isBackBufferToResize = false;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred width of the back buffer.
        /// </summary>
        /// <value>The preferred width of the back buffer.</value>
        public int PreferredBackBufferWidth
        {
            get => preferredBackBufferWidth;

            set
            {
                if (preferredBackBufferWidth != value)
                {
                    preferredBackBufferWidth = value;
                    isBackBufferToResize = false;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred depth stencil format.
        /// </summary>
        /// <value>The preferred depth stencil format.</value>
        public PixelFormat PreferredDepthStencilFormat
        {
            get => preferredDepthStencilFormat;

            set
            {
                if (preferredDepthStencilFormat != value)
                {
                    preferredDepthStencilFormat = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the preferred refresh rate.
        /// </summary>
        /// <value>The preferred refresh rate.</value>
        public Rational PreferredRefreshRate
        {
            get => preferredRefreshRate;

            set
            {
                if (preferredRefreshRate != value)
                {
                    preferredRefreshRate = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the output (monitor) index to use when switching to fullscreen mode.
        /// </summary>
        /// <remarks>
        ///   Doesn't have any effect when windowed mode is used.
        /// </remarks>
        public int PreferredFullScreenOutputIndex
        {
            get => preferredFullScreenOutputIndex;

            set
            {
                if (preferredFullScreenOutputIndex != value)
                {
                    preferredFullScreenOutputIndex = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the supported orientations.
        /// </summary>
        /// <value>The supported orientations.</value>
        public DisplayOrientation SupportedOrientations
        {
            get => supportedOrientations;

            set
            {
                if (supportedOrientations != value)
                {
                    supportedOrientations = value;
                    deviceSettingsChanged = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to synchronize drawing with the vertical retrace, also known
        ///   as VSync.
        /// </summary>
        /// <value><c>true</c> to synchronize with vertical retrace; otherwise, <c>false</c>.</value>
        public bool SynchronizeWithVerticalRetrace
        {
            get => synchronizeWithVerticalRetrace;

            set
            {
                if (synchronizeWithVerticalRetrace != value)
                {
                    synchronizeWithVerticalRetrace = value;
                    deviceSettingsChanged = true;
                }
            }
        }


        /// <summary>
        ///   Applies the changes made to the configuration and changes or creates the <see cref="GraphicsDevice"/>
        ///   according to the new values.
        /// </summary>
        public void ApplyChanges()
        {
            if (GraphicsDevice != null && deviceSettingsChanged)
            {
                ChangeOrCreateDevice(forceCreate: false);
            }
        }

        bool IGraphicsDeviceManager.BeginDraw()
        {
            if (GraphicsDevice is null)
                return false;

            beginDrawOk = false;

            if (!CheckDeviceState())
                return false;

            GraphicsDevice.Begin();

            // TODO: GRAPHICS REFACTOR
            //   Before drawing, we should clear the state to make sure that there is no unstable graphics device states (On some WP8 devices for example)
            //   An application should not rely on previous state (last frame...etc.) after BeginDraw.
            //GraphicsDevice.ClearState();
            //
            //// By default, we setup the render target to the back buffer, and the viewport as well.
            //if (GraphicsDevice.BackBuffer != null)
            //{
            //    GraphicsDevice.SetDepthAndRenderTarget(GraphicsDevice.DepthStencilBuffer, GraphicsDevice.BackBuffer);
            //}

            beginDrawOk = true;
            return beginDrawOk;
        }

        private bool CheckDeviceState()
        {
            switch (GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Removed:
                    Utilities.Sleep(TimeSpan.FromMilliseconds(20));
                    return false;

                case GraphicsDeviceStatus.Reset:
                    Utilities.Sleep(TimeSpan.FromMilliseconds(20));
                    try
                    {
                        ChangeOrCreateDevice(true);
                    }
                    catch
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        void IGraphicsDeviceManager.CreateDevice()
        {
            // Force the creation of the device
            ChangeOrCreateDevice(forceCreate: true);
        }

        void IGraphicsDeviceManager.EndDraw(bool present)
        {
            if (beginDrawOk && GraphicsDevice != null)
            {
                if (present && GraphicsDevice.Presenter != null)
                {
                    try
                    {
                        GraphicsDevice.Presenter.Present();
                    }
                    catch (GraphicsException ex)
                    {
                        // If this is not a DeviceRemoved or DeviceReset, then throw an exception
                        if (ex.Status != GraphicsDeviceStatus.Removed &&
                            ex.Status != GraphicsDeviceStatus.Reset)
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        beginDrawOk = false;
                        GraphicsDevice.End();
                    }
                }
                else
                {
                    beginDrawOk = false;
                    GraphicsDevice.End();
                }
            }
        }


        protected static DisplayOrientation SelectOrientation(DisplayOrientation orientation, int width, int height, bool allowLandscapeLeftAndRight)
        {
            if (orientation != DisplayOrientation.Default)
            {
                return orientation;
            }

            if (width <= height)
            {
                return DisplayOrientation.Portrait;
            }

            if (allowLandscapeLeftAndRight)
            {
                return DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft;
            }

            return DisplayOrientation.LandscapeRight;
        }

        protected override void Destroy()
        {
            if (game != null)
            {
                if (game.Services.GetService<IGraphicsDeviceService>() == this)
                    game.Services.RemoveService<IGraphicsDeviceService>();

                if (game.Services.GetService<IGraphicsDeviceManager>() == this)
                    game.Services.RemoveService<IGraphicsDeviceManager>();

                game.WindowCreated -= GameOnWindowCreated;
                if (game.Window != null)
                {
                    game.Window.ClientSizeChanged -= Window_ClientSizeChanged;
                    game.Window.OrientationChanged -= Window_OrientationChanged;
                }
            }

            if (GraphicsDevice != null)
            {
                if (GraphicsDevice.Presenter != null)
                {
                    // Make sure that the Presenter is reverted to window before shuting down
                    // otherwise the Direct3D11.Device will generate an exception on Dispose()
                    GraphicsDevice.Presenter.IsFullScreen = false;
                    GraphicsDevice.Presenter.Dispose();
                    GraphicsDevice.Presenter = null;
                }

                //GraphicsDevice.DeviceResetting -= GraphicsDevice_DeviceResetting;
                //GraphicsDevice.DeviceReset -= GraphicsDevice_DeviceReset;
                //GraphicsDevice.DeviceLost -= GraphicsDevice_DeviceLost;

                GraphicsDevice.Dispose();
                GraphicsDevice.Disposing -= GraphicsDevice_Disposing;
                GraphicsDevice = null;
            }

            base.Destroy();
        }

        /// <summary>
        ///   Determines whether this instance is compatible with the the specified new <see cref="GraphicsDeviceInformation"/>.
        /// </summary>
        /// <param name="newDeviceInfo">The new device info.</param>
        /// <returns><c>true</c> if this instance is compatible with the the specified <see cref="GraphicsDeviceInformation"/>; otherwise, <c>false</c>.</returns>
        protected virtual bool CanResetDevice(GraphicsDeviceInformation newDeviceInfo)
        {
            // By default, a reset is compatible when we stay under the same graphics profile.
            return GraphicsDevice.Features.RequestedProfile == newDeviceInfo.GraphicsProfile;
        }

        /// <summary>
        ///   Finds the best device that is compatible with the preferences defined in the configuration.
        /// </summary>
        /// <param name="anySuitableDevice">
        ///   <c>true</c> to select a device from any existing adapters;
        ///   <c>false</c> to select only from the default adapter.
        /// </param>
        /// <returns>The graphics device information.</returns>
        protected virtual GraphicsDeviceInformation FindBestDevice(bool anySuitableDevice)
        {
            // Setup preferred parameters before passing them to the factory
            var preferredParameters = new GameGraphicsParameters
                {
                    PreferredBackBufferWidth = PreferredBackBufferWidth,
                    PreferredBackBufferHeight = PreferredBackBufferHeight,
                    PreferredBackBufferFormat = PreferredBackBufferFormat,
                    PreferredDepthStencilFormat = PreferredDepthStencilFormat,
                    PreferredRefreshRate = PreferredRefreshRate,
                    PreferredFullScreenOutputIndex = PreferredFullScreenOutputIndex,
                    IsFullScreen = IsFullScreen,
                    PreferredMultisampleCount = PreferredMultisampleCount,
                    SynchronizeWithVerticalRetrace = SynchronizeWithVerticalRetrace,
                    PreferredGraphicsProfile = (GraphicsProfile[]) PreferredGraphicsProfile.Clone(),
                    ColorSpace = PreferredColorSpace,
                    RequiredAdapterUid = RequiredAdapterUid,
            };

            // Remap to sRGB backbuffer if necessary
            if (PreferredColorSpace == ColorSpace.Linear)
            {
                // If the device support sRGB and ColorSpace is linear, we use automatically a sRGB backbuffer
                preferredParameters.PreferredBackBufferFormat = preferredParameters.PreferredBackBufferFormat.ToSRgb();
            }
            else
            {
                // If we are looking for gamma and the backbuffer format is sRGB, switch back to non-sRGB
                preferredParameters.PreferredBackBufferFormat = preferredParameters.PreferredBackBufferFormat.ToNonSRgb();
            }

            // Setup resized value if there is a resize pending
            if (!IsFullScreen && isBackBufferToResize)
            {
                preferredParameters.PreferredBackBufferWidth = resizedBackBufferWidth;
                preferredParameters.PreferredBackBufferHeight = resizedBackBufferHeight;
            }

            var devices = graphicsDeviceFactory.FindBestDevices(preferredParameters);
            if (devices.Count == 0)
            {
                // Nothing was found; first, let's check if graphics profile was actually supported
                // NOTE: We don't do this preemptively because in some cases it seems to take lot of time (happened on a test machine, several seconds freeze on ID3D11Device.Release())
                if (!IsPreferredProfileAvailable(preferredParameters.PreferredGraphicsProfile, out GraphicsProfile availableGraphicsProfile))
                    throw new InvalidOperationException($"Graphics profiles [{string.Join(", ", preferredParameters.PreferredGraphicsProfile)}] are not supported by the device. The highest available profile is [{availableGraphicsProfile}].");

                // Otherwise, there was just no screen mode
                throw new InvalidOperationException("No screen modes found.");
            }

            RankDevices(devices);

            if (devices.Count == 0)
                throw new InvalidOperationException("No screen modes found after ranking.");

            return devices[0];
        }

        /// <summary>
        ///   Ranks a list of <see cref="GraphicsDeviceInformation"/> before creating a new device.
        /// </summary>
        /// <param name="foundDevices">The list of devices that can be reordered.</param>
        protected virtual void RankDevices(List<GraphicsDeviceInformation> foundDevices)
        {
            // Don't sort if there is a single device (mostly for XAML/WP8)
            if (foundDevices.Count == 1)
                return;

            foundDevices.Sort(
                (left, right) =>
                    {
                        var leftParams = left.PresentationParameters;
                        var rightParams = right.PresentationParameters;

                        var leftAdapter = left.Adapter;
                        var rightAdapter = right.Adapter;

                        // Sort by GraphicsProfile
                        if (left.GraphicsProfile != right.GraphicsProfile)
                        {
                            return left.GraphicsProfile <= right.GraphicsProfile ? 1 : -1;
                        }

                        // Sort by FullScreen mode
                        if (leftParams.IsFullScreen != rightParams.IsFullScreen)
                        {
                            return IsFullScreen != leftParams.IsFullScreen ? 1 : -1;
                        }

                        // Sort by BackBufferFormat
                        int leftFormat = CalculateRankForFormat(leftParams.BackBufferFormat);
                        int rightFormat = CalculateRankForFormat(rightParams.BackBufferFormat);
                        if (leftFormat != rightFormat)
                        {
                            return leftFormat >= rightFormat ? 1 : -1;
                        }

                        // Sort by MultisampleCount
                        if (leftParams.MultisampleCount != rightParams.MultisampleCount)
                        {
                            return leftParams.MultisampleCount <= rightParams.MultisampleCount ? 1 : -1;
                        }

                        // Sort by AspectRatio
                        var targetAspectRatio = (PreferredBackBufferWidth == 0) || (PreferredBackBufferHeight == 0) ? (float)DefaultBackBufferWidth / DefaultBackBufferHeight : (float)PreferredBackBufferWidth / PreferredBackBufferHeight;
                        var leftDiffRatio = Math.Abs(((float)leftParams.BackBufferWidth / leftParams.BackBufferHeight) - targetAspectRatio);
                        var rightDiffRatio = Math.Abs(((float)rightParams.BackBufferWidth / rightParams.BackBufferHeight) - targetAspectRatio);
                        if (Math.Abs(leftDiffRatio - rightDiffRatio) > 0.2f)
                        {
                            return leftDiffRatio >= rightDiffRatio ? 1 : -1;
                        }

                        // Sort by PixelCount
                        int leftPixelCount;
                        int rightPixelCount;
                        if (IsFullScreen)
                        {
                            if (((PreferredBackBufferWidth == 0) || (PreferredBackBufferHeight == 0)) &&
                                PreferredFullScreenOutputIndex < leftAdapter.Outputs.Length &&
                                PreferredFullScreenOutputIndex < rightAdapter.Outputs.Length)
                            {
                                // assume we got here only adapters that have the needed number of outputs:
                                var leftOutput = leftAdapter.Outputs[PreferredFullScreenOutputIndex];
                                var rightOutput = rightAdapter.Outputs[PreferredFullScreenOutputIndex];

                                leftPixelCount = leftOutput.CurrentDisplayMode.Width * leftOutput.CurrentDisplayMode.Height;
                                rightPixelCount = rightOutput.CurrentDisplayMode.Width * rightOutput.CurrentDisplayMode.Height;
                            }
                            else
                            {
                                leftPixelCount = rightPixelCount = PreferredBackBufferWidth * PreferredBackBufferHeight;
                            }
                        }
                        else if ((PreferredBackBufferWidth == 0) || (PreferredBackBufferHeight == 0))
                        {
                            leftPixelCount = rightPixelCount = DefaultBackBufferWidth * DefaultBackBufferHeight;
                        }
                        else
                        {
                            leftPixelCount = rightPixelCount = PreferredBackBufferWidth * PreferredBackBufferHeight;
                        }

                        int leftDeltaPixelCount = Math.Abs((leftParams.BackBufferWidth * leftParams.BackBufferHeight) - leftPixelCount);
                        int rightDeltaPixelCount = Math.Abs((rightParams.BackBufferWidth * rightParams.BackBufferHeight) - rightPixelCount);
                        if (leftDeltaPixelCount != rightDeltaPixelCount)
                        {
                            return leftDeltaPixelCount >= rightDeltaPixelCount ? 1 : -1;
                        }

                        // Sort by default Adapter, default adapter first
                        if (left.Adapter != right.Adapter)
                        {
                            if (left.Adapter.IsDefaultAdapter)
                            {
                                return -1;
                            }

                            if (right.Adapter.IsDefaultAdapter)
                            {
                                return 1;
                            }
                        }

                        return 0;
                    });
        }

        protected virtual bool IsPreferredProfileAvailable(GraphicsProfile[] preferredProfiles, out GraphicsProfile availableProfile)
        {
            availableProfile = GraphicsProfile.Level_11_0;

            var graphicsProfiles = Enum.GetValues(typeof(GraphicsProfile));

            foreach (var graphicsAdapter in GraphicsAdapterFactory.Adapters)
            {
                foreach (var graphicsProfileValue in graphicsProfiles)
                {
                    var graphicsProfile = (GraphicsProfile) graphicsProfileValue;
                    if (graphicsProfile > availableProfile && graphicsAdapter.IsProfileSupported(graphicsProfile))
                        availableProfile = graphicsProfile;
                }
            }

            foreach (var preferredProfile in preferredProfiles)
            {
                if (availableProfile >= preferredProfile)
                    return true;
            }

            return false;
        }

        private int CalculateRankForFormat(PixelFormat format)
        {
            if (format == PreferredBackBufferFormat)
                return 0;

            if (CalculateFormatSize(format) == CalculateFormatSize(PreferredBackBufferFormat))
                return 1;

            return int.MaxValue;
        }

        private int CalculateFormatSize(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R8G8B8A8_UNorm:
                case PixelFormat.R8G8B8A8_UNorm_SRgb:
                case PixelFormat.B8G8R8A8_UNorm:
                case PixelFormat.B8G8R8A8_UNorm_SRgb:
                case PixelFormat.R10G10B10A2_UNorm:
                    return 32;

                case PixelFormat.B5G6R5_UNorm:
                case PixelFormat.B5G5R5A1_UNorm:
                    return 16;
            }

            return 0;
        }

        protected virtual void OnDeviceCreated(object sender, EventArgs args)
        {
            DeviceCreated?.Invoke(sender, args);
        }

        protected virtual void OnDeviceDisposing(object sender, EventArgs args)
        {
            DeviceDisposing?.Invoke(sender, args);
        }

        protected virtual void OnDeviceReset(object sender, EventArgs args)
        {
            DeviceReset?.Invoke(sender, args);
        }

        protected virtual void OnDeviceResetting(object sender, EventArgs args)
        {
            DeviceResetting?.Invoke(sender, args);
        }

        protected virtual void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args)
        {
            PreparingDeviceSettings?.Invoke(sender, args);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Rectangle clientBounds = game.Window.ClientBounds;

            if (!isChangingDevice &&
                (clientBounds.Height != 0 || clientBounds.Width != 0))
            {
                resizedBackBufferWidth = clientBounds.Width;
                resizedBackBufferHeight = clientBounds.Height;
                isBackBufferToResize = true;

                if (GraphicsDevice != null)
                    ChangeOrCreateDevice(false);
            }
        }

        private void Window_OrientationChanged(object sender, EventArgs e)
        {
            Rectangle clientBounds = game.Window.ClientBounds;

            if (!isChangingDevice &&
                (clientBounds.Height != 0 || clientBounds.Width != 0) &&
                (game.Window.CurrentOrientation != currentWindowOrientation))
            {
                if ((clientBounds.Height > clientBounds.Width && preferredBackBufferWidth > preferredBackBufferHeight) ||
                    (clientBounds.Width > clientBounds.Height && preferredBackBufferHeight > preferredBackBufferWidth))
                {
                    // Client size and Back Buffer size are different things
                    // In this case all we care is if orientation changed, if so we swap width and height
                    (PreferredBackBufferWidth, PreferredBackBufferHeight) = (PreferredBackBufferHeight, PreferredBackBufferWidth);
                    ApplyChanges();
                }
            }
        }

        private void Window_FullscreenChanged(object sender, EventArgs eventArgs)
        {
            if (sender is GameWindow window)
            {
                IsFullScreen = window.IsFullscreen;
                if (IsFullScreen)
                {
                    PreferredBackBufferWidth = window.PreferredFullscreenSize.X;
                    PreferredBackBufferHeight = window.PreferredFullscreenSize.Y;
                }
                else
                {
                    PreferredBackBufferWidth = window.PreferredWindowedSize.X;
                    PreferredBackBufferHeight = window.PreferredWindowedSize.Y;
                }

                ApplyChanges();
            }
        }

        private void CreateDevice(GraphicsDeviceInformation newInfo)
        {
            newInfo.PresentationParameters.IsFullScreen = isFullScreen;
            newInfo.PresentationParameters.PresentationInterval = SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate;
            newInfo.DeviceCreationFlags = DeviceCreationFlags;

            // this.ValidateGraphicsDeviceInformation(newInfo);

            bool deviceRecreate = GraphicsDevice != null;

            // Notify device is resetting (usually this should result in graphics resources being destroyed)
            if (deviceRecreate)
                OnDeviceResetting(this, EventArgs.Empty);

            // Create (or recreate) the graphics device
            GraphicsDevice = graphicsDeviceFactory.ChangeOrCreateDevice(GraphicsDevice, newInfo);

            // Notify device is reset (usually this should result in graphics resources being recreated/reloaded)
            if (deviceRecreate)
                OnDeviceReset(this, EventArgs.Empty);

            // Use the shader profile returned by the GraphicsDeviceInformation otherwise use the one coming from the GameSettings
            GraphicsDevice.ShaderProfile = ShaderProfile;

            // TODO: HANDLE Device Resetting/Reset/Lost
            //GraphicsDevice.DeviceResetting += GraphicsDevice_DeviceResetting;
            //GraphicsDevice.DeviceReset += GraphicsDevice_DeviceReset;
            //GraphicsDevice.DeviceLost += GraphicsDevice_DeviceLost;
            if (!deviceRecreate)
                GraphicsDevice.Disposing += GraphicsDevice_Disposing;

            OnDeviceCreated(this, EventArgs.Empty);
        }

        private void GraphicsDevice_DeviceResetting(object sender, EventArgs e)
        {
            // TODO: What to do?
        }

        private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            // TODO: What to do?
        }

        private void GraphicsDevice_DeviceLost(object sender, EventArgs e)
        {
            // TODO: What to do?
        }

        private void GraphicsDevice_Disposing(object sender, EventArgs e)
        {
            OnDeviceDisposing(sender, e);
        }

        private void ChangeOrCreateDevice(bool forceCreate)
        {
            // We make sure that we won't be call by an asynchronous event (windows resized)
            lock (lockDeviceCreation)
            {
                using (Profiler.Begin(GraphicsDeviceManagerProfilingKeys.CreateDevice))
                {
                    // If Device is null we assume we are still at game creation phase
                    game.ConfirmRenderingSettings(GraphicsDevice is null);

                    isChangingDevice = true;
                    var width = game.Window.ClientBounds.Width;
                    var height = game.Window.ClientBounds.Height;

                    // If the orientation is free to be changed from portrait to landscape we actually need this check now,
                    // it is mostly useful only at initialization actually tho because Window_OrientationChanged does the same logic on runtime change
                    if (game.Window.CurrentOrientation != currentWindowOrientation)
                    {
                        if ((game.Window.ClientBounds.Height > game.Window.ClientBounds.Width && preferredBackBufferWidth > preferredBackBufferHeight) ||
                            (game.Window.ClientBounds.Width > game.Window.ClientBounds.Height && preferredBackBufferHeight > preferredBackBufferWidth))
                        {
                            // Client size and Back Buffer size are different things
                            // In this case all we care is if orientation changed, if so we swap width and height
                            (preferredBackBufferWidth, preferredBackBufferHeight) = (preferredBackBufferHeight, preferredBackBufferWidth);
                        }
                    }

                    var isBeginScreenDeviceChange = false;
                    try
                    {
                        // Notifies the game window for the new orientation
                        var orientation = SelectOrientation(supportedOrientations, PreferredBackBufferWidth, PreferredBackBufferHeight, true);
                        game.Window.SetSupportedOrientations(orientation);

                        var graphicsDeviceInformation = FindBestDevice(forceCreate);

                        OnPreparingDeviceSettings(this, new PreparingDeviceSettingsEventArgs(graphicsDeviceInformation));

                        isFullScreen = graphicsDeviceInformation.PresentationParameters.IsFullScreen;
                        game.Window.BeginScreenDeviceChange(graphicsDeviceInformation.PresentationParameters.IsFullScreen);
                        isBeginScreenDeviceChange = true;
                        bool needToCreateNewDevice = true;

                        // If we are not forced to create a new device and this is already an existing GraphicsDevice
                        // try to reset and resize it.
                        if (!forceCreate && GraphicsDevice != null)
                        {
                            if (CanResetDevice(graphicsDeviceInformation))
                            {
                                try
                                {
                                    var newWidth = graphicsDeviceInformation.PresentationParameters.BackBufferWidth;
                                    var newHeight = graphicsDeviceInformation.PresentationParameters.BackBufferHeight;
                                    var newFormat = graphicsDeviceInformation.PresentationParameters.BackBufferFormat;
                                    var newOutputIndex = graphicsDeviceInformation.PresentationParameters.PreferredFullScreenOutputIndex;

                                    GraphicsDevice.Presenter.Description.PreferredFullScreenOutputIndex = newOutputIndex;
                                    GraphicsDevice.Presenter.Description.RefreshRate = graphicsDeviceInformation.PresentationParameters.RefreshRate;
                                    GraphicsDevice.Presenter.Resize(newWidth, newHeight, newFormat);

                                    // Change full screen if needed
                                    GraphicsDevice.Presenter.IsFullScreen = graphicsDeviceInformation.PresentationParameters.IsFullScreen;

                                    needToCreateNewDevice = false;
                                }
                                catch
                                {
                                    // Ignored
                                }
                            }
                        }

                        // If we still need to create a device, then we need to create it
                        if (needToCreateNewDevice)
                            CreateDevice(graphicsDeviceInformation);

                        if (GraphicsDevice is null)
                            throw new InvalidOperationException("Unexpected null GraphicsDevice.");

                        // Make sure to copy back color space to GraphicsDevice
                        GraphicsDevice.ColorSpace = graphicsDeviceInformation.PresentationParameters.ColorSpace;

                        var presentationParameters = GraphicsDevice.Presenter.Description;
                        isReallyFullScreen = presentationParameters.IsFullScreen;
                        if (presentationParameters.BackBufferWidth != 0)
                            width = presentationParameters.BackBufferWidth;
                        if (presentationParameters.BackBufferHeight != 0)
                            height = presentationParameters.BackBufferHeight;

                        deviceSettingsChanged = false;
                    }
                    finally
                    {
                        if (isBeginScreenDeviceChange)
                        {
                            game.Window.EndScreenDeviceChange(width, height);
                            game.Window.SetIsReallyFullscreen(isReallyFullScreen);
                        }

                        currentWindowOrientation = game.Window.CurrentOrientation;
                        isChangingDevice = false;
                    }
                }
            }
        }
    }
}
