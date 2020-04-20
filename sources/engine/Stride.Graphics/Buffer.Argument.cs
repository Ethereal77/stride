// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Argument buffer helper methods.
        /// </summary>
        public static class Argument
        {
            /// <summary>
            /// Creates a new Argument buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Argument buffer</returns>
            public static Buffer New(GraphicsDevice device, int size, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
            {
                return Buffer.New(device, size, BufferFlags.ArgumentBuffer, usage);
            }

            /// <summary>
            /// Creates a new Argument buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Argument buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Argument buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, 1, BufferFlags.ArgumentBuffer, usage);
            }

            /// <summary>
            /// Creates a new Argument buffer with <see cref="GraphicsResourceUsage.Default"/> uasge by default.
            /// </summary>
            /// <typeparam name="T">Type of the Argument buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Argument buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Argument buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, ref T value, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.ArgumentBuffer, usage);
            }
        }
    }
}
