// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_GRAPHICS_API_DIRECT3D

using System;
using System.Reflection;

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
    /// Graphics presenter for SwapChain.
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
                if (swapChain == null)
                    return;

                var outputIndex = Description.PreferredFullScreenOutputIndex;

                // no outputs connected to the current graphics adapter
                var output = GraphicsDevice.Adapter != null &&
                             outputIndex < GraphicsDevice.Adapter.Outputs.Length
                                ? GraphicsDevice.Adapter.Outputs[outputIndex] : null;

                Output currentOutput = null;

                try
                {
                    RawBool isCurrentlyFullscreen;
                    swapChain.GetFullscreenState(out isCurrentlyFullscreen, out currentOutput);

                    // check if the current fullscreen monitor is the same as new one
                    // If not fullscreen, currentOutput will be null but output won't be, so don't compare them
                    if (isCurrentlyFullscreen == value && (isCurrentlyFullscreen == false || (output != null && currentOutput != null && currentOutput.NativePointer == output.NativeOutput.NativePointer)))
                        return;
                }
                finally
                {
                    currentOutput?.Dispose();
                }

                bool switchToFullScreen = value;
                // If going to fullscreen mode: call 1) SwapChain.ResizeTarget 2) SwapChain.IsFullScreen
                var description = new ModeDescription(backBuffer.ViewWidth, backBuffer.ViewHeight, Description.RefreshRate.ToSharpDX(), (SharpDX.DXGI.Format)Description.BackBufferFormat);
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

        public override void BeginDraw(CommandList commandList)
        {
        }

        public override void EndDraw(CommandList commandList, bool present)
        {
        }

        public override void Present()
        {
            try
            {
                swapChain.Present((int)PresentInterval, PresentFlags.None);
#if STRIDE_GRAPHICS_API_DIRECT3D12
                // Manually swap back buffer
                backBuffer.NativeResource.Dispose();
                backBuffer.InitializeFromImpl(swapChain.GetBackBuffer<BackBufferResourceType>((++bufferSwapIndex) % bufferCount), Description.BackBufferFormat.IsSRgb());
#endif
            }
            catch (SharpDXException sharpDxException)
            {
                var deviceStatus = GraphicsDevice.GraphicsDeviceStatus;
                throw new GraphicsException($"Unexpected error on Present (device status: {deviceStatus})", sharpDxException, deviceStatus);
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

            swapChain.ResizeBuffers(bufferCount, width, height, (SharpDX.DXGI.Format)format, SwapChainFlags.None);

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
        /// Calls <see cref="Texture.OnDestroyed"/> for all children of the specified texture
        /// </summary>
        /// <param name="parentTexture">Specified parent texture</param>
        /// <returns>A list of the children textures which were destroyed</returns>
        private FastList<Texture> DestroyChildrenTextures(Texture parentTexture)
        {
            var fastList = new FastList<Texture>();
            foreach (var resource in GraphicsDevice.Resources)
            {
                var texture = resource as Texture;
                if (texture != null && texture.ParentTexture == parentTexture)
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
            if (Description.DeviceWindowHandle == null)
            {
                throw new ArgumentException("DeviceWindowHandle cannot be null");
            }

            return CreateSwapChainForWindows();
        }

        /// <summary>
        /// Create the SwapChain on Windows. To avoid any hard dependency on a actual windowing system
        /// we assume that the <c>Description.DeviceWindowHandle.NativeHandle</c> holds
        /// a window type that exposes the <code>Handle</code> property of type <see cref="IntPtr"/>.
        /// </summary>
        /// <returns></returns>
        private SwapChain CreateSwapChainForWindows()
        {
            var nativeHandle = Description.DeviceWindowHandle.NativeWindow;
            var handleProperty = nativeHandle.GetType().GetProperty("Handle", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (handleProperty != null && handleProperty.PropertyType == typeof(IntPtr))
            {
                var hwndPtr = (IntPtr)handleProperty.GetValue(nativeHandle);
                if (hwndPtr != IntPtr.Zero)
                {
                    return CreateSwapChainForDesktop(hwndPtr);
                }
            }
            throw new NotSupportedException($"Form of type [{Description.DeviceWindowHandle?.GetType().Name ?? "null"}] is not supported. Only System.Windows.Control is supported.");
        }

        private SwapChain CreateSwapChainForDesktop(IntPtr handle)
        {
            bufferCount = 1;
            var backbufferFormat = Description.BackBufferFormat;
#if STRIDE_GRAPHICS_API_DIRECT3D12
            // TODO D3D12 (check if this setting make sense on D3D11 too?)
            backbufferFormat = backbufferFormat.ToNonSRgb();
            // TODO D3D12 Can we make it work with something else after?
            bufferCount = 2;
#endif
            var description = new SwapChainDescription
                {
                    ModeDescription = new ModeDescription(Description.BackBufferWidth, Description.BackBufferHeight, Description.RefreshRate.ToSharpDX(), (SharpDX.DXGI.Format)backbufferFormat),
                    BufferCount = bufferCount, // TODO: Do we really need this to be configurable by the user?
                    OutputHandle = handle,
                    SampleDescription = new SampleDescription((int)Description.MultisampleCount, 0),
#if STRIDE_GRAPHICS_API_DIRECT3D11
                    SwapEffect = SwapEffect.Discard,
#elif STRIDE_GRAPHICS_API_DIRECT3D12
                    SwapEffect = SwapEffect.FlipDiscard,
#endif
                    Usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput,
                    IsWindowed = true,
                    Flags = Description.IsFullScreen ? SwapChainFlags.AllowModeSwitch : SwapChainFlags.None,
                };

#if STRIDE_GRAPHICS_API_DIRECT3D11
            var newSwapChain = new SwapChain(GraphicsAdapterFactory.NativeFactory, GraphicsDevice.NativeDevice, description);
#elif STRIDE_GRAPHICS_API_DIRECT3D12
            var newSwapChain = new SwapChain(GraphicsAdapterFactory.NativeFactory, GraphicsDevice.NativeCommandQueue, description);
#endif

            //prevent normal alt-tab
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
