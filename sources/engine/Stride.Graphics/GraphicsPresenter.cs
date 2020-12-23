// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Reoresents a class that abstracts a swap chain, a collection of backbuffers that are flipped or copied to
    ///   a frontbuffer to show the rendered image on the screen.
    /// </summary>
    /// <remarks>
    ///   In order to create a new <see cref="GraphicsPresenter"/>, a <see cref="Graphics.GraphicsDevice"/> should have
    ///   been initialized first.
    /// </remarks>
    public abstract class GraphicsPresenter : ComponentBase
    {
        /// <summary>
        ///   A presentation interval to use forcefully during a <see cref="Present"/> operation.
        /// </summary>
        /// <remarks>
        ///   If the value is <c>null</c>, the original <see cref="Graphics.PresentInterval"/> will be used. If not
        ///   <c>null</c>, this value will be used instead.
        ///   <para/>
        ///   This is currently only supported by the Direct3D graphics implementation.
        /// </remarks>
        internal static readonly PropertyKey<PresentInterval?> ForcedPresentInterval = new PropertyKey<PresentInterval?>(nameof(ForcedPresentInterval), typeof(GraphicsDevice));

        private Texture depthStencilBuffer;

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsPresenter" /> class.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="presentationParameters">The parameters that describes how to present graphics to the screen.</param>
        protected GraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters)
        {
            GraphicsDevice = device;
            var description = presentationParameters.Clone();

            description.BackBufferFormat = NormalizeBackBufferFormat(description.BackBufferFormat);

            Description = description;

            ProcessPresentationParameters();

            // Creates a default DepthStencilBuffer.
            CreateDepthStencilBuffer();
        }

        /// <summary>
        ///   Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        ///   Gets the description of this presenter.
        /// </summary>
        /// <value>
        ///   A <see cref="PresentationParameters"/> describing how to present graphics to the screen.
        /// </value>
        public PresentationParameters Description { get; private set; }

        /// <summary>
        ///   Gets the default back buffer for this presenter.
        /// </summary>
        public abstract Texture BackBuffer { get; }

        /// <summary>
        ///   Gets the default depth stencil buffer for this presenter.
        /// </summary>
        public Texture DepthStencilBuffer
        {
            get => depthStencilBuffer;
            protected set => depthStencilBuffer = value;
        }

        /// <summary>
        ///   Gets the underlying native presenter.
        /// </summary>
        /// <value>The native presenter.</value>
        /// <remarks>
        ///   This can be a <see cref="SharpDX.DXGI.SwapChain"/> or <see cref="SharpDX.DXGI.SwapChain1"/> or <see langword="null"/>,
        ///   depending on the platform).
        /// </remarks>
        public abstract object NativePresenter { get; }

        /// <summary>
        ///   Gets or sets a value indicating whether this presenter should render in fullscreen mode.
        /// </summary>
        /// <value><c>true</c> to present in full screen; otherwise, <c>false</c>.</value>
        public abstract bool IsFullScreen { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref="Graphics.PresentInterval"/>.
        /// </summary>
        /// <value>
        ///   The interval to wait before presenting a frame to the screen. Default is to wait for one
        ///   vertical blanking (VSync).
        /// </value>
        public PresentInterval PresentInterval
        {
            get => Description.PresentationInterval;
            set => Description.PresentationInterval = value;
        }

        public virtual void BeginDraw(CommandList commandList) { }

        public virtual void EndDraw(CommandList commandList, bool present) { }

        /// <summary>
        ///   Presents the backbuffer to the screen.
        /// </summary>
        public abstract void Present();

        /// <summary>
        ///   Resizes the current presenter, resizing the back buffer and the depth stencil buffer.
        /// </summary>
        /// <param name="width">New width of the resized backbuffer.</param>
        /// <param name="height">New height of the resized backbuffer.</param>
        /// <param name="format">New pixel format of the backbuffer.</param>
        public void Resize(int width, int height, PixelFormat format)
        {
            GraphicsDevice.Begin();

            Description.BackBufferWidth = width;
            Description.BackBufferHeight = height;
            Description.BackBufferFormat = NormalizeBackBufferFormat(format);

            ResizeBackBuffer(width, height, format);
            ResizeDepthStencilBuffer(width, height, format);

            GraphicsDevice.End();
        }

        private PixelFormat NormalizeBackBufferFormat(PixelFormat backBufferFormat)
        {
            if (GraphicsDevice.Features.HasSRgb && GraphicsDevice.ColorSpace == ColorSpace.Linear)
            {
                // If the device support SRgb and ColorSpace is linear, we use automatically a SRgb backbuffer
                return backBufferFormat.ToSRgb();
            }
            else
            {
                // If the device does not support SRgb or the ColorSpace is Gamma, but the backbuffer format asked is SRgb, convert it to non SRgb
                return backBufferFormat.ToNonSRgb();
            }
        }

        protected abstract void ResizeBackBuffer(int width, int height, PixelFormat format);

        protected abstract void ResizeDepthStencilBuffer(int width, int height, PixelFormat format);

        protected void ReleaseCurrentDepthStencilBuffer()
        {
            if (DepthStencilBuffer != null)
            {
                depthStencilBuffer.RemoveDisposeBy(this);
            }
        }

        protected override void Destroy()
        {
            OnDestroyed();

            base.Destroy();
        }

        /// <summary>
        ///   Method called when this presenter has been destroyed.
        /// </summary>
        protected internal virtual void OnDestroyed() { }

        /// <summary>
        ///   Method called when this presenter has been recreated.
        /// </summary>
        public virtual void OnRecreated() { }

        protected virtual void ProcessPresentationParameters() { }

        /// <summary>
        ///   Creates the depth stencil buffer.
        /// </summary>
        protected virtual void CreateDepthStencilBuffer()
        {
            // If no depth stencil buffer, just return
            if (Description.DepthStencilFormat == PixelFormat.None)
                return;

            // Creates the depth stencil buffer.
            var flags = TextureFlags.DepthStencil;
            if (Description.MultisampleCount == MultisampleCount.None)
            {
                flags |= TextureFlags.ShaderResource;
            }

            // Create texture description
            var depthTextureDescription = TextureDescription.New2D(Description.BackBufferWidth, Description.BackBufferHeight, Description.DepthStencilFormat, flags);
            depthTextureDescription.MultisampleCount = Description.MultisampleCount;

            var depthTexture = Texture.New(GraphicsDevice, depthTextureDescription);
            DepthStencilBuffer = depthTexture.DisposeBy(this);
        }
    }
}
