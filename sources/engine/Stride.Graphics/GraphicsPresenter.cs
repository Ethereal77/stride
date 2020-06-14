// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;
using Stride.Core.ReferenceCounting;

namespace Stride.Graphics
{
    /// <summary>
    ///   Reoresents a class that abstracts a
    /// This class is a frontend to <see cref="SwapChain" /> and <see cref="SwapChain1" />.
    /// </summary>
    /// <remarks>
    /// In order to create a new <see cref="GraphicsPresenter"/>, a <see cref="GraphicsDevice"/> should have been initialized first.
    /// </remarks>
    public abstract class GraphicsPresenter : ComponentBase
    {
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

            // If we are creating a GraphicsPresenter with linear colorspace
            if (device.Features.HasSRgb && device.ColorSpace == ColorSpace.Linear)
            {
                // We use automatically a SRgb backbuffer
                if (description.BackBufferFormat == PixelFormat.R8G8B8A8_UNorm)
                    description.BackBufferFormat = PixelFormat.R8G8B8A8_UNorm_SRgb;
                else if (description.BackBufferFormat == PixelFormat.B8G8R8A8_UNorm)
                    description.BackBufferFormat = PixelFormat.B8G8R8A8_UNorm_SRgb;
            }
            else if (!device.Features.HasSRgb)
            {
                // The device does not support SRgb, but the backbuffer format asked is SRgb, convert it to non SRgb
                if (description.BackBufferFormat == PixelFormat.R8G8B8A8_UNorm_SRgb)
                    description.BackBufferFormat = PixelFormat.R8G8B8A8_UNorm;
                else if (description.BackBufferFormat == PixelFormat.B8G8R8A8_UNorm_SRgb)
                    description.BackBufferFormat = PixelFormat.B8G8R8A8_UNorm;
            }

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
        ///   Gets or sets the <see cref="PresentInterval"/>. Default is to wait for one vertical blanking (VSync).
        /// </summary>
        /// <value>The present interval.</value>
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
            Description.BackBufferFormat = format;

            ResizeBackBuffer(width, height, format);
            ResizeDepthStencilBuffer(width, height, format);

            GraphicsDevice.End();
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
        ///   Methid called when this presenter has been destroyed.
        /// </summary>
        protected internal virtual void OnDestroyed() { }

        /// <summary>
        ///   Methid called when this presenter has been recreated.
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
