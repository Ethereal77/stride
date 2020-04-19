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
        /// Typed buffer helper methods.
        /// </summary>
        /// <remarks>
        /// Example in HLSL: Buffer&lt;float4&gt;.
        /// </remarks>
        public static class Typed
        {
            /// <summary>
            /// Creates a new Typed buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="count">The number of data with the following viewFormat.</param>
            /// <param name="viewFormat">The view format of the buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Typed buffer</returns>
            public static Buffer New(GraphicsDevice device, int count, PixelFormat viewFormat, bool isUnorderedAccess = false, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
            {
                return Buffer.New(device, count * viewFormat.SizeInBytes(), BufferFlags.ShaderResource | (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None), viewFormat, usage);
            }

            /// <summary>
            /// Creates a new Typed buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <typeparam name="T">Type of the Typed buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Typed buffer.</param>
            /// <param name="viewFormat">The view format of the buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Typed buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value, PixelFormat viewFormat, bool isUnorderedAccess = false, GraphicsResourceUsage usage = GraphicsResourceUsage.Default) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.ShaderResource | (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None), viewFormat, usage);
            }

            /// <summary>
            /// Creates a new Typed buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Typed buffer.</param>
            /// <param name="viewFormat">The view format of the buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Typed buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, PixelFormat viewFormat, bool isUnorderedAccess = false, GraphicsResourceUsage usage = GraphicsResourceUsage.Default)
            {
                return Buffer.New(device, value, 0, BufferFlags.ShaderResource | (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None), viewFormat, usage);
            }
        }
    }
}
