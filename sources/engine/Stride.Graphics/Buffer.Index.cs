// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Games;

namespace Stride.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Index buffer helper methods.
        /// </summary>
        public static class Index
        {
            /// <summary>
            /// Creates a new index buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A index buffer</returns>
            public static Buffer New(GraphicsDevice device, int size, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
            {
                return Buffer.New(device, size, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the index buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A index buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, 1, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="GraphicsResourceUsage.Immutable"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the index buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, ref T value, GraphicsResourceUsage usage = GraphicsResourceUsage.Immutable) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="GraphicsResourceUsage.Immutable"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the index buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value, GraphicsResourceUsage usage = GraphicsResourceUsage.Immutable) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="GraphicsResourceUsage.Immutable"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="is32BitIndex">Set to true if the buffer is using a 32 bit index or false for 16 bit index.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer New(GraphicsDevice device, byte[] value, bool is32BitIndex, GraphicsResourceUsage usage = GraphicsResourceUsage.Immutable)
            {
                return Buffer.New(device, value, is32BitIndex ? 4 : 2, BufferFlags.IndexBuffer, PixelFormat.None, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="GraphicsResourceUsage.Immutable"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, GraphicsResourceUsage usage = GraphicsResourceUsage.Immutable)
            {
                return Buffer.New(device, value, 0, BufferFlags.IndexBuffer, usage);
            }
        }
    }
}
