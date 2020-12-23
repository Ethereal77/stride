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
    ///   Represents an abstract window for a <see cref="GameBase"/> to render on and process inputs.
    /// </summary>
    public abstract class GameWindow : ComponentBase
    {
        private string title;

        /// <summary>
        /// Indicate if the window is currently activated.
        /// </summary>
        public bool IsActivated;

        #region Public Events

        /// <summary>
        ///   Occurs when this window is activated.
        /// </summary>
        public event EventHandler<EventArgs> Activated;

        /// <summary>
        ///   Occurs when the window client size has changed.
        /// </summary>
        public event EventHandler<EventArgs> ClientSizeChanged;

        /// <summary>
        ///   Occurs when this window is deactivated.
        /// </summary>
        public event EventHandler<EventArgs> Deactivated;

        /// <summary>
        ///   Occurs when device orientation has changed.
        /// </summary>
        public event EventHandler<EventArgs> OrientationChanged;

        /// <summary>
        ///   Occurs when the device fullscreen mode has changed.
        /// </summary>
        public event EventHandler<EventArgs> FullscreenChanged;

        /// <summary>
        ///   Occurs when the window is being closed, before it gets destroyed.
        /// </summary>
        public event EventHandler<EventArgs> Closing;

        #endregion

        /// <summary>
        ///   Gets or sets a value indicating whether the user can resize this window.
        /// </summary>
        public abstract bool AllowUserResizing { get; set; }

        /// <summary>
        ///   Gets the client bounds of the window.
        /// </summary>
        /// <value>The client bounds.</value>
        public abstract Rectangle ClientBounds { get; }

        /// <summary>
        ///   Gets the current orientation.
        /// </summary>
        /// <value>The current orientation.</value>
        public abstract DisplayOrientation CurrentOrientation { get; }

        /// <summary>
        ///   Gets a value indicating whether this window is minimized.
        /// </summary>
        /// <value><c>true</c> if the window is minimized; otherwise, <c>false</c>.</value>
        public abstract bool IsMinimized { get; }

        /// <summary>
        ///   Gets a value indicating whether this window has focus.
        /// </summary>
        /// <value><c>true</c> if the window is in focus; otherwise, <c>false</c>.</value>
        public abstract bool Focused { get; }

        /// <summary>
        ///   Gets or sets a value indicating whether the mouse pointer is visible over this window.
        /// </summary>
        /// <value><c>true</c> if the mouse pointer is visible; otherwise, <c>false</c>.</value>
        public abstract bool IsMouseVisible { get; set; }

        /// <summary>
        ///   Gets the native window handle.
        /// </summary>
        /// <value>The native window.</value>
        public abstract WindowHandle NativeWindow { get; }

        /// <summary>
        ///   Gets or sets a value indicating whether this window is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public abstract bool Visible { get; set; }

        /// <summary>
        ///   Gets or sets the position of the window on the screen.
        /// </summary>
        public virtual Int2 Position { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this window has a border.
        /// </summary>
        /// <value><c>true</c> if this window has a border; otherwise, <c>false</c>.</value>
        public abstract bool IsBorderLess { get; set; }

        /// <summary>
        ///   Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get => title;

            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Title));

                if (title != value)
                {
                    title = value;
                    SetTitle(title);
                }
            }
        }

        /// <summary>
        ///   Gets or sets the size the window should have when switching from fullscreen to windowed mode.
        /// </summary>
        /// <remarks>
        ///   To get the current actual size use <see cref="ClientBounds"/>.
        ///   This gets overwritten when the user resizes the window.
        /// </remarks>
        public Int2 PreferredWindowedSize { get; set; } = new Int2(768, 432);

        /// <summary>
        ///   Gets or sets the size the window should have when switching from windowed to fullscreen mode.
        /// </summary>
        /// <remarks>
        ///   To get the current actual size use <see cref="ClientBounds"/>.
        /// </remarks>
        public Int2 PreferredFullscreenSize { get; set; } = new Int2(1920, 1080);

        /// <summary>
        ///   Gets or sets a value indicating whether the fullscreen mode should be a borderless window matching the
        ///   desktop size.
        /// </summary>
        /// <remarks>
        ///   This flag is currently ignored on all game platforms other than SDL.
        /// </remarks>
        public bool FullscreenIsBorderlessWindow { get; set; } = false;

        /// <summary>
        ///   Gets or sets a value indicating whether to switch to fullscreen or windowed mode.
        /// </summary>
        /// <value><c>true</c> for fullscreen mode. <c>false</c> for windowed mode.</value>
        public bool IsFullscreen
        {
            get => isFullscreen;

            set
            {
                if (value != isFullscreen)
                {
                    isFullscreen = value;
                    FullscreenChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   Allows the GraphicsDeviceMagnager to set the actual window state after applying the device changes.
        /// </summary>
        /// <param name="isReallyFullscreen">Device configuration about fullscreen.</param>
        internal void SetIsReallyFullscreen(bool isReallyFullscreen)
        {
            isFullscreen = isReallyFullscreen;
        }


        public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

        public void EndScreenDeviceChange()
        {
            EndScreenDeviceChange(ClientBounds.Width, ClientBounds.Height);
        }

        public abstract void EndScreenDeviceChange(int clientWidth, int clientHeight);


        protected internal abstract void Initialize(GameContext gameContext);

        internal bool Exiting;

        internal Action InitCallback;
        internal Action RunCallback;
        internal Action ExitCallback;

        private bool isFullscreen;

        internal abstract void Run();

        /// <summary>
        ///   Sets the size of the client area and triggers the <see cref="ClientSizeChanged"/> event.
        ///   This will trigger a backbuffer resize too.
        /// </summary>
        public void SetSize(Int2 size)
        {
            Resize(size.X, size.Y);
            OnClientSizeChanged(this, EventArgs.Empty);
        }

        /// <summary>
        ///   Only used internally by the device managers when they adapt the window size to the backbuffer size.
        ///   Resizes the window, without sending the resized event.
        /// </summary>
        internal abstract void Resize(int width, int height);

        public virtual IMessageLoop CreateUserManagedMessageLoop()
        {
            // Default: not implemented
            throw new PlatformNotSupportedException();
        }

        internal IServiceRegistry Services { get; set; }

        protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

        protected void OnActivated(object source, EventArgs e)
        {
            IsActivated = true;

            var handler = Activated;
            handler?.Invoke(source, e);
        }

        protected void OnClientSizeChanged(object source, EventArgs e)
        {
            if (!isFullscreen)
            {
                // Update preferred windowed size in windowed mode
                var resizeSize = ClientBounds.Size;
                PreferredWindowedSize = new Int2(resizeSize.Width, resizeSize.Height);
            }
            var handler = ClientSizeChanged;
            handler?.Invoke(this, e);
        }

        protected void OnDeactivated(object source, EventArgs e)
        {
            IsActivated = false;

            var handler = Deactivated;
            handler?.Invoke(source, e);
        }

        protected void OnOrientationChanged(object source, EventArgs e)
        {
            var handler = OrientationChanged;
            handler?.Invoke(this, e);
        }

        protected void OnFullscreenToggle(object source, EventArgs e)
        {
            IsFullscreen = !IsFullscreen;
        }

        protected void OnClosing(object source, EventArgs e)
        {
            var handler = Closing;
            handler?.Invoke(this, e);
        }

        protected abstract void SetTitle(string title);

        internal void OnPause()
        {
            OnDeactivated(this, EventArgs.Empty);
        }

        internal void OnResume()
        {
            OnActivated(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///   Represents an abstract <see cref="GameWindow"/> that renders and processes the input of a specific
    ///   type of control.
    /// </summary>
    /// <typeparam name="TControl">Type of the control to use as window.</typeparam>
    public abstract class GameWindow<TControl> : GameWindow
    {
        protected internal sealed override void Initialize(GameContext gameContext)
        {
            if (gameContext is GameContext<TControl> context)
            {
                GameContext = context;
                Initialize(context);
            }
            else
                throw new InvalidOperationException("Invalid context for the current game.");
        }

        internal GameContext<TControl> GameContext;

        protected abstract void Initialize(GameContext<TControl> context);
    }
}
