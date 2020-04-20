// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Graphics
{
    /// <summary>
    /// Graphics presenter for SwapChain.
    /// </summary>
    public class RenderTargetGraphicsPresenter : GraphicsPresenter
    {
        private Texture backBuffer;

        public RenderTargetGraphicsPresenter(GraphicsDevice device, Texture renderTarget, PixelFormat depthFormat = PixelFormat.None)
            : base(device, CreatePresentationParameters(renderTarget, depthFormat))
        {
            PresentInterval = Description.PresentationInterval;
            // Initialize the swap chain
            SetBackBuffer(renderTarget);
        }

        private static PresentationParameters CreatePresentationParameters(Texture renderTarget2D, PixelFormat depthFormat)
        {
            return new PresentationParameters()
                {
                    BackBufferWidth = renderTarget2D.Width,
                    BackBufferHeight = renderTarget2D.Height,
                    BackBufferFormat = renderTarget2D.ViewFormat,
                    DepthStencilFormat = depthFormat,
                    DeviceWindowHandle = null,
                    IsFullScreen = true,
                    MultisampleCount = renderTarget2D.MultisampleCount,
                    PresentationInterval = PresentInterval.One,
                    RefreshRate = new Rational(60, 1),
                };
        }

        public override Texture BackBuffer
        {
            get
            {
                return backBuffer;
            }
        }

        /// <summary>
        /// Sets the back buffer.
        /// </summary>
        /// <param name="backBuffer">The back buffer.</param>
        public void SetBackBuffer(Texture backBuffer)
        {
            this.backBuffer = backBuffer.EnsureRenderTarget();
        }

        public override object NativePresenter
        {
            get
            {
                return backBuffer;
            }
        }

        public override bool IsFullScreen
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        public override void Present()
        {
        }

        protected override void ResizeBackBuffer(int width, int height, PixelFormat format)
        {
            throw new System.NotImplementedException();
        }

        protected override void ResizeDepthStencilBuffer(int width, int height, PixelFormat format)
        {
            throw new System.NotImplementedException();
        }
    }
}
