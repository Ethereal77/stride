// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Games;

namespace Xenko.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Constant buffer helper methods.
        /// </summary>
        public static class Constant
        {
            /// <summary>
            /// Creates a new constant buffer with a default <see cref="GraphicsResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer New(GraphicsDevice device, int size, GraphicsResourceUsage usage = GraphicsResourceUsage.Dynamic)
            {
                return Buffer.New(device, size, BufferFlags.ConstantBuffer, usage);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="GraphicsResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the constant buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device) where T : struct
            {
                return Buffer.New<T>(device, 1, BufferFlags.ConstantBuffer, GraphicsResourceUsage.Dynamic);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="GraphicsResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the constant buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the constant buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, ref T value, GraphicsResourceUsage usage = GraphicsResourceUsage.Dynamic) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.ConstantBuffer, usage);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="GraphicsResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the constant buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the constant buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value, GraphicsResourceUsage usage = GraphicsResourceUsage.Dynamic) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.ConstantBuffer, usage);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="GraphicsResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the constant buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, GraphicsResourceUsage usage = GraphicsResourceUsage.Dynamic)
            {
                return Buffer.New(device, value, 0, BufferFlags.ConstantBuffer, usage);
            }
        }
    }
}
