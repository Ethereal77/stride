// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D

using System;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

using Stride.Core.Collections;

#if STRIDE_GRAPHICS_API_DIRECT3D11
using BackBufferResourceType = SharpDX.Direct3D11.Texture2D;
#elif STRIDE_GRAPHICS_API_DIRECT3D12
using BackBufferResourceType = SharpDX.Direct3D12.Resource;
#endif

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents a <see cref="GraphicsPresenter"/> implementation for a DXGI SwapChain.
    /// </summary>
    public class SwapChainGraphicsPresenter : GraphicsPresenter
    {
        private SwapChain swapChain;

        private readonly Texture backBuffer;

        private int bufferCount;

#if STRIDE_GRAPHICS_API_DIRECT3D12
        private int bufferSwapIndex;
#endif

        public SwapChainGraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters)
            : base(device, presentationParameters)
        {
            PresentInterval = presentationParameters.PresentationInterval;

            // Initialize the swap chain
            swapChain = CreateSwapChain();

            backBuffer = new Texture(device).InitializeFromImpl(swapChain.GetBackBuffer<BackBufferResourceType>(0), Description.BackBufferFormat.IsSRgb());

            // Reload should get backbuffer from swapchain as well
            //backBufferTexture.Reload = graphicsResource => ((Texture)graphicsResource).Recreate(swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture>(0));
        }

        public override Texture BackBuffer => backBuffer;

        public override object NativePresenter => swapChain;

        public override bool IsFullScreen
        {
            get => swapChain.IsFullScreen;

            set
            {
                if (swapChain is null)
                    return;

                var outputIndex = Description.PreferredFullScreenOutputIndex;

                // No outputs connected to the current graphics adapter
                var output = GraphicsDevice.Adapter != null &&
                             outputIndex < GraphicsDevice.Adapter.Outputs.Length ?
                                GraphicsDevice.Adapter.Outputs[outputIndex] :
                                null;

                Output currentOutput = null;

                try
                {
                    swapChain.GetFullscreenState(out RawBool isCurrentlyFullscreen, out currentOutput);

                    // Check if the current fullscreen monitor is the same as the new one.
                    // If not fullscreen, currentOutput will be null but output won't be, so don't compare them
                    if (isCurrentlyFullscreen == value &&
                        (isCurrentlyFullscreen == false || (output != null && currentOutput != null && currentOutput.NativePointer == output.NativeOutput.NativePointer)))
                        return;
                }
                finally
                {
                    currentOutput?.Dispose();
                }

                // If going to fullscreen mode: call 1) SwapChain.ResizeTarget 2) SwapChain.IsFullScreen
                var description = new ModeDescription(
                    backBuffer.ViewWidth,
                    backBuffer.ViewHeight,
                    Description.RefreshRate.ToSharpDX(),
                    (Format) Description.BackBufferFormat);
                bool switchToFullScreen = value;
                if (switchToFullScreen)
                {
                    OnDestroyed();

                    Description.IsFullScreen = true;

                    OnRecreated();
                }
                else
                {
                    Description.IsFullScreen = false;
                    swapChain.IsFullScreen = false;

                    // call 1) SwapChain.IsFullScreen 2) SwapChain.Resize
                    Resize(backBuffer.ViewWidth, backBuffer.ViewHeight, backBuffer.ViewFormat);
                }

                // If going to window mode:
                if (!switchToFullScreen)
                {
                    // call 1) SwapChain.IsFullScreen 2) SwapChain.Resize
                    description.RefreshRate = new SharpDX.DXGI.Rational(0, 0);
                    swapChain.ResizeTarget(ref description);
                }
            }
        }

        public override void BeginDraw(CommandList commandList) { }

        public override void EndDraw(CommandList commandList, bool present) { }

        public override void Present()
        {
            try
            {
                var presentInterval = GraphicsDevice.Tags.Get(ForcedPresentInterval) ?? PresentInterval;

                swapChain.Present((int) presentInterval, PresentFlags.None);

#if STRIDE_GRAPHICS_API_DIRECT3D12
                // Manually swap back buffer
                backBuffer.NativeResource.Dispose();
                backBuffer.InitializeFromImpl(swapChain.GetBackBuffer<BackBufferResourceType>((++bufferSwapIndex) % bufferCount), Description.BackBufferFormat.IsSRgb());
#endif
            }
            catch (SharpDXException sharpDxException)
            {
                var deviceStatus = GraphicsDevice.GraphicsDeviceStatus;
                throw new GraphicsException($"Unexpected error on Present (device status: {deviceStatus}).", sharpDxException, deviceStatus);
            }
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();

            if (Name != null && GraphicsDevice != null && GraphicsDevice.IsDebugMode && swapChain != null)
            {
                swapChain.DebugName = Name;
            }
        }

        protected internal override void OnDestroyed()
        {
            // Manually update back buffer texture
            backBuffer.OnDestroyed();
            backBuffer.LifetimeState = GraphicsResourceLifetimeState.Destroyed;

            swapChain.Dispose();
            swapChain = null;

            base.OnDestroyed();
        }

        public override void OnRecreated()
        {
            base.OnRecreated();

            // Recreate swap chain
            swapChain = CreateSwapChain();

            // Get newly created native texture
            var backBufferTexture = swapChain.GetBackBuffer<BackBufferResourceType>(0);

            // Put it in our back buffer texture
            // TODO: Update new size
            backBuffer.InitializeFromImpl(backBufferTexture, Description.BackBufferFormat.IsSRgb());
            backBuffer.LifetimeState = GraphicsResourceLifetimeState.Active;
        }

        protected override void ResizeBackBuffer(int width, int height, PixelFormat format)
        {
            // Manually update back buffer texture
            backBuffer.OnDestroyed();

            // Manually update all children textures
            var fastList = DestroyChildrenTextures(backBuffer);

            // If format is same as before, using Unknown (None) will keep the current
            // We do that because on Win10/RT, actual format might be the non-srgb one and we don't want to switch to srgb one by mistake (or need #ifdef)
            if (format == backBuffer.Format)
                format = PixelFormat.None;

            swapChain.ResizeBuffers(bufferCount, width, height, (Format) format, SwapChainFlags.None);

            // Get newly created native texture
            var backBufferTexture = swapChain.GetBackBuffer<BackBufferResourceType>(0);

            // Put it in our back buffer texture
            backBuffer.InitializeFromImpl(backBufferTexture, Description.BackBufferFormat.IsSRgb());

            foreach (var texture in fastList)
            {
                texture.InitializeFrom(backBuffer, texture.ViewDescription);
            }
        }

        protected override void ResizeDepthStencilBuffer(int width, int height, PixelFormat format)
        {
            var newTextureDescription = DepthStencilBuffer.Description;
            newTextureDescription.Width = width;
            newTextureDescription.Height = height;

            // Manually update the texture
            DepthStencilBuffer.OnDestroyed();

            // Manually update all children textures
            var fastList = DestroyChildrenTextures(DepthStencilBuffer);

            // Put it in our back buffer texture
            DepthStencilBuffer.InitializeFrom(newTextureDescription);

            foreach (var texture in fastList)
            {
                texture.InitializeFrom(DepthStencilBuffer, texture.ViewDescription);
            }
        }

        /// <summary>
        ///   Calls <see cref="Texture.OnDestroyed"/> for all children of the specified texture.
        /// </summary>
        /// <param name="parentTexture">Specified parent texture.</param>
        /// <returns>A list of the children textures which were destroyed.</returns>
        private FastList<Texture> DestroyChildrenTextures(Texture parentTexture)
        {
            var fastList = new FastList<Texture>();
            foreach (var resource in GraphicsDevice.Resources)
            {
                if (resource is Texture texture && texture.ParentTexture == parentTexture)
                {
                    texture.OnDestroyed();
                    fastList.Add(texture);
                }
            }

            return fastList;
        }

        private SwapChain CreateSwapChain()
        {
            // Check for Window Handle parameter
            if (Description.DeviceWindowHandle is null)
                throw new ArgumentException("DeviceWindowHandle cannot be null.");

            return CreateSwapChainForWindows();
        }

        /// <summary>
        ///   Create the SwapChain on Windows.
        /// </summary>
        /// <returns></returns>
        private SwapChain CreateSwapChainForWindows()
        {
            var hwndPtr = Description.DeviceWindowHandle.Handle;
            if (hwndPtr != IntPtr.Zero)
                return CreateSwapChainForDesktop(hwndPtr);

            throw new InvalidOperationException($"The {nameof(WindowHandle)}.{nameof(WindowHandle.Handle)} must not be zero.");
        }

        private SwapChain CreateSwapChainForDesktop(IntPtr handle)
        {
            bufferCount = 1;
            var backbufferFormat = Description.BackBufferFormat;

#if STRIDE_GRAPHICS_API_DIRECT3D12
            // TODO: D3D12 (check if this setting make sense on D3D11 too?)
            backbufferFormat = backbufferFormat.ToNonSRgb();
            // TODO: D3D12 Can we make it work with something else after?
            bufferCount = 2;
#endif
            var description = new SwapChainDescription
                {
                    ModeDescription = new ModeDescription(
                        Description.BackBufferWidth,
                        Description.BackBufferHeight,
                        Description.RefreshRate.ToSharpDX(),
                        (Format) backbufferFormat),
                    BufferCount = bufferCount, // TODO: Do we really need this to be configurable by the user?
                    OutputHandle = handle,
                    SampleDescription = new SampleDescription((int) Description.MultisampleCount, 0),
#if STRIDE_GRAPHICS_API_DIRECT3D11
                    SwapEffect = SwapEffect.Discard,
#elif STRIDE_GRAPHICS_API_DIRECT3D12
                    SwapEffect = SwapEffect.FlipDiscard,
#endif
                    Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
                    IsWindowed = true,
                    Flags = Description.IsFullScreen ? SwapChainFlags.AllowModeSwitch : SwapChainFlags.None,
                };

#if STRIDE_GRAPHICS_API_DIRECT3D11
            var newSwapChain = new SwapChain(GraphicsAdapterFactory.NativeFactory, GraphicsDevice.NativeDevice, description);
#elif STRIDE_GRAPHICS_API_DIRECT3D12
            var newSwapChain = new SwapChain(GraphicsAdapterFactory.NativeFactory, GraphicsDevice.NativeCommandQueue, description);
#endif

            // Prevent normal Alt + Enter
            GraphicsAdapterFactory.NativeFactory.MakeWindowAssociation(handle, WindowAssociationFlags.IgnoreAltEnter);

            if (Description.IsFullScreen)
            {
                // Before fullscreen switch
                newSwapChain.ResizeTarget(ref description.ModeDescription);

                // Switch to full screen
                newSwapChain.IsFullScreen = true;

                // This is really important to call ResizeBuffers AFTER switching to IsFullScreen
                newSwapChain.ResizeBuffers(bufferCount, Description.BackBufferWidth, Description.BackBufferHeight, (SharpDX.DXGI.Format)Description.BackBufferFormat, SwapChainFlags.AllowModeSwitch);
            }

            return newSwapChain;
        }
    }
}
#endif
