// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.


#if STRIDE_GRAPHICS_API_DIRECT3D11 || STRIDE_GRAPHICS_API_DIRECT3D12

using SharpDX;
#if STRIDE_GRAPHICS_API_DIRECT3D11
using SharpDX.Direct3D11;
#elif STRIDE_GRAPHICS_API_DIRECT3D12
using SharpDX.Direct3D12;
#endif

namespace Stride.Graphics
{
    public static class SharpDXInterop
    {
        /// <summary>
        ///   Gets the native device (DX11/DX12).
        /// </summary>
        /// <param name="device">The Stride <see cref="GraphicsDevice"/> instance.</param>
        /// <returns>An object representing the native device object.</returns>
        public static object GetNativeDevice(GraphicsDevice device) => GetNativeDeviceImpl(device);

        /// <summary>
        ///   Gets the native device context (DX11).
        /// </summary>
        /// <param name="device">The Stride <see cref="GraphicsDevice"/> instance.</param>
        /// <returns>An object representing the native device context object.</returns>
        public static object GetNativeDeviceContext(GraphicsDevice device) => GetNativeDeviceContextImpl(device);

        /// <summary>
        ///   Gets the native command queue (DX12 only).
        /// </summary>
        /// <param name="device">The Stride <see cref="GraphicsDevice"/> instance.</param>
        /// <returns>An object representing the native command queue object.</returns>
        public static object GetNativeCommandQueue(GraphicsDevice device) => GetNativeCommandQueueImpl(device);

        /// <summary>
        ///   Gets the native resource handle of a DirectX 11 resource.
        /// </summary>
        /// <param name="resource">The Stride <see cref="GraphicsResource"/> instance.</param>
        /// <returns>An object representing the native graphics resource object.</returns>
        public static object GetNativeResource(GraphicsResource resource) => GetNativeResourceImpl(resource);

        /// <summary>
        ///   Gets the native shader resource view of a resource.
        /// </summary>
        /// <param name="resource">The Stride <see cref="GraphicsResource"/> instance.</param>
        /// <returns>An object representing the native shader resource view object.</returns>
        public static object GetNativeShaderResourceView(GraphicsResource resource) => GetNativeShaderResourceViewImpl(resource);

        /// <summary>
        ///   Gets the native render target view of a texture.
        /// </summary>
        /// <param name="texture">The Stride <see cref="Texture"/> instance.</param>
        /// <returns>An object representing the native render target view object.</returns>
        public static object GetNativeRenderTargetView(Texture texture) => GetNativeRenderTargetViewImpl(texture);

        /// <summary>
        ///   Creates a texture from a DirectX 11 native texture.
        /// </summary>
        /// <param name="device">The Stride <see cref="GraphicsDevice"/> instance.</param>
        /// <param name="dxTexture2D">The DX11 texture</param>
        /// <param name="takeOwnership">
        ///   A value indicating whether to take ownership of the native texture.
        ///   If <c>false</c>, <see cref="IUnknown.AddReference"/> will be called on the texture.
        /// </param>
        /// <param name="isSRgb">A value indicating whether to set the format to sRGB.</param>
        /// <returns>A <see cref="Texture"/> encapsulating the native DirectX 11 texture.</returns>
        /// <remarks>
        ///   This method internally will call <see cref="IUnknown.AddReference"/> on the <paramref name="dxTexture2D"/> texture.
        /// </remarks>
        public static Texture CreateTextureFromNative(GraphicsDevice device, object dxTexture2D, bool takeOwnership, bool isSRgb = false)
        {
#if STRIDE_GRAPHICS_API_DIRECT3D11
            return CreateTextureFromNativeImpl(device, (Texture2D) dxTexture2D, takeOwnership, isSRgb);
#elif STRIDE_GRAPHICS_API_DIRECT3D12
            return CreateTextureFromNativeImpl(device, (Resource) dxTexture2D, takeOwnership, isSRgb);
#endif
        }

#if STRIDE_GRAPHICS_API_DIRECT3D11

        //
        // Gets the DirectX 11 native device.
        //
        private static Device GetNativeDeviceImpl(GraphicsDevice device) => device.NativeDevice;

        //
        // Gets the DirectX 11 native device context.
        //
        private static DeviceContext GetNativeDeviceContextImpl(GraphicsDevice device) => device.NativeDeviceContext;

        //
        // Returns null as DirectX 11 doesn't have the concept of a native command queue.
        //
        private static object GetNativeCommandQueueImpl(GraphicsDevice _) => null;

        //
        // Gets the DirectX 11 native resource handle.
        //
        private static Resource GetNativeResourceImpl(GraphicsResource resource) => resource.NativeResource;

        //
        // Gets the DirectX 11 native shader resource view from a resource.
        //
        private static ShaderResourceView GetNativeShaderResourceViewImpl(GraphicsResource resource) => resource.NativeShaderResourceView;

        //
        // Gets the DirectX 11 native render target view from a texture.
        //
        private static RenderTargetView GetNativeRenderTargetViewImpl(Texture texture) => texture.NativeRenderTargetView;

        //
        // Creates a Texture instance encapsulating a DirectX 11 texture.
        //
        private static Texture CreateTextureFromNativeImpl(GraphicsDevice device, Texture2D dxTexture2D, bool takeOwnership, bool isSRgb = false)
        {
            var tex = new Texture(device);

            if (takeOwnership)
            {
                var unknown = dxTexture2D as IUnknown;
                unknown.AddReference();
            }

            tex.InitializeFromImpl(dxTexture2D, isSRgb);

            return tex;
        }

#elif STRIDE_GRAPHICS_API_DIRECT3D12

        //
        // Gets the DirectX 12 native device.
        //
        private static Device GetNativeDeviceImpl(GraphicsDevice device) => device.NativeDevice;

        //
        // Returns null as DirectX 12 doesn't have the concept of a native device context.
        //
        private static object GetNativeDeviceContextImpl(GraphicsDevice device) => null;

        //
        // Gets the DirectX 12 native command queue.
        //
        private static CommandQueue GetNativeCommandQueueImpl(GraphicsDevice device) => device.NativeCommandQueue;

        //
        // Gets the DirectX 12 native resource handle.
        //
        private static Resource GetNativeResourceImpl(GraphicsResource resource) => resource.NativeResource;

        //
        // Gets the DirectX 12 native shader resource view from a resource.
        //
        private static CpuDescriptorHandle GetNativeShaderResourceViewImpl(GraphicsResource resource) => resource.NativeShaderResourceView;

        //
        // Gets the DirectX 12 native render target view from a texture.
        //
        private static CpuDescriptorHandle GetNativeRenderTargetViewImpl(Texture texture) => texture.NativeRenderTargetView;

        //
        // Creates a Texture instance encapsulating a DirectX 12 texture.
        //
        private static Texture CreateTextureFromNativeImpl(GraphicsDevice device, Resource dxTexture2D, bool takeOwnership, bool isSRgb = false)
        {
            var tex = new Texture(device);

            if (takeOwnership)
            {
                var unknown = dxTexture2D as IUnknown;
                unknown.AddReference();
            }

            tex.InitializeFromImpl(dxTexture2D, isSRgb);

            return tex;
        }
#endif

    }
}

#endif
