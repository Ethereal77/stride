// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Games;

namespace Stride.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Raw buffer helper methods.
        /// </summary>
        /// <remarks>
        /// Example in HLSL: ByteAddressBuffer or RWByteAddressBuffer for raw buffers supporting unordered access.
        /// </remarks>
        public static class Raw
        {
            /// <summary>
            /// Creates a new Raw buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="additionalBindings">The additional bindings (for example, to create a combined raw/index buffer, pass <see cref="BufferFlags.IndexBuffer" />)</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Raw buffer</returns>
            public static Buffer New(GraphicsDevice device, int size, BufferFlags additionalBindings = BufferFlags.None, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
            {
                return Buffer.New(device, size, BufferFlags.RawBuffer | additionalBindings, usage);
            }

            /// <summary>
            /// Creates a new Raw buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Raw buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="additionalBindings">The additional bindings (for example, to create a combined raw/index buffer, pass <see cref="BufferFlags.IndexBuffer" />)</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Raw buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, BufferFlags additionalBindings = BufferFlags.None, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, 1, BufferFlags.RawBuffer | additionalBindings, usage);
            }

            /// <summary>
            /// Creates a new Raw buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Raw buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Raw buffer.</param>
            /// <param name="additionalBindings">The additional bindings (for example, to create a combined raw/index buffer, pass <see cref="BufferFlags.IndexBuffer" />)</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Raw buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, ref T value, BufferFlags additionalBindings = BufferFlags.None, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.RawBuffer | additionalBindings, usage);
            }

            /// <summary>
            /// Creates a new Raw buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Raw buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Raw buffer.</param>
            /// <param name="additionalBindings">The additional bindings (for example, to create a combined raw/index buffer, pass <see cref="BufferFlags.IndexBuffer" />)</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Raw buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value, BufferFlags additionalBindings = BufferFlags.None, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.RawBuffer | additionalBindings, usage);
            }

            /// <summary>
            /// Creates a new Raw buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Raw buffer.</param>
            /// <param name="additionalBindings">The additional bindings (for example, to create a combined raw/index buffer, pass <see cref="BufferFlags.IndexBuffer" />)</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Raw buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, BufferFlags additionalBindings = BufferFlags.None, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
            {
                return Buffer.New(device, value, 0, BufferFlags.RawBuffer | additionalBindings, usage);
            }
        }
    }
}
