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
        /// Structured buffer helper methods.
        /// </summary>
        /// <remarks>
        /// Example in HLSL: StructuredBuffer&lt;float4&gt; or RWStructuredBuffer&lt;float4&gt for structured buffers supporting unordered access.
        /// </remarks>
        public static class Structured
        {
            /// <summary>
            /// Creates a new Structured buffer accessible as a <see cref="ShaderResourceView" /> and optionaly as a <see cref="UnorderedAccessView" />.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="count">The number of element in this buffer.</param>
            /// <param name="elementSize">Size of the struct.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <returns>A Structured buffer</returns>
            public static Buffer New(GraphicsDevice device, int count, int elementSize, bool isUnorderedAccess = false)
            {
                var bufferFlags = BufferFlags.StructuredBuffer | BufferFlags.ShaderResource;

                if (isUnorderedAccess)
                    bufferFlags |= BufferFlags.UnorderedAccess;

                return Buffer.New(device, count * elementSize, elementSize, bufferFlags);
            }

            /// <summary>
            /// Creates a new Structured buffer accessible as a <see cref="ShaderResourceView" /> and optionaly as a <see cref="UnorderedAccessView" />.
            /// </summary>
            /// <typeparam name="T">Type of the element in the structured buffer</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="count">The number of element in this buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <returns>A Structured buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, int count, bool isUnorderedAccess = false) where T : struct
            {
                var bufferFlags = BufferFlags.StructuredBuffer | BufferFlags.ShaderResource;

                if (isUnorderedAccess)
                    bufferFlags |= BufferFlags.UnorderedAccess;

                return Buffer.New<T>(device, count, bufferFlags);
            }

            /// <summary>
            /// Creates a new Structured buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <typeparam name="T">Type of the Structured buffer to get the sizeof from</typeparam>
            /// <param name="value">The value to initialize the Structured buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <returns>A Structured buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value, bool isUnorderedAccess = false) where T : struct
            {
                var bufferFlags = BufferFlags.StructuredBuffer | BufferFlags.ShaderResource;

                if (isUnorderedAccess)
                    bufferFlags |= BufferFlags.UnorderedAccess;

                return Buffer.New(device, value, bufferFlags);
            }

            /// <summary>
            /// Creates a new Structured buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the Structured buffer.</param>
            /// <param name="elementSize">Size of the element.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <returns>A Structured buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, int elementSize, bool isUnorderedAccess = false)
            {
                var bufferFlags = BufferFlags.StructuredBuffer | BufferFlags.ShaderResource;

                if (isUnorderedAccess)
                    bufferFlags |= BufferFlags.UnorderedAccess;

                return Buffer.New(device, value, elementSize, bufferFlags);
            }
        }

        /// <summary>
        /// StructuredAppend buffer helper methods.
        /// </summary>
        /// <remarks>
        /// Example in HLSL: AppendStructuredBuffer&lt;float4&gt; or ConsumeStructuredBuffer&lt;float4&gt.
        /// </remarks>
        public static class StructuredAppend
        {
            /// <summary>
            /// Creates a new StructuredAppend buffer accessible as a <see cref="ShaderResourceView" /> and as a <see cref="UnorderedAccessView" />.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="count">The number of element in this buffer.</param>
            /// <param name="elementSize">Size of the struct.</param>
            /// <returns>A StructuredAppend buffer</returns>
            public static Buffer New(GraphicsDevice device, int count, int elementSize)
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredAppendBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New(device, count * elementSize, elementSize, BufferFlags);
            }

            /// <summary>
            /// Creates a new StructuredAppend buffer accessible as a <see cref="ShaderResourceView" /> and optionaly as a <see cref="UnorderedAccessView" />.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <typeparam name="T">Type of the element in the structured buffer</typeparam>
            /// <param name="count">The number of element in this buffer.</param>
            /// <returns>A Structured buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, int count) where T : struct
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredAppendBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New<T>(device, count, BufferFlags);
            }

            /// <summary>
            /// Creates a new StructuredAppend buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <typeparam name="T">Type of the StructuredAppend buffer to get the sizeof from</typeparam>
            /// <param name="value">The value to initialize the StructuredAppend buffer.</param>
            /// <returns>A StructuredAppend buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, T[] value) where T : struct
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredAppendBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New(device, value, BufferFlags);
            }

            /// <summary>
            /// Creates a new StructuredAppend buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the StructuredAppend buffer.</param>
            /// <param name="elementSize">Size of the element.</param>
            /// <returns>A StructuredAppend buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, int elementSize)
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredAppendBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New(device, value, elementSize, BufferFlags);
            }
        }

        /// <summary>
        /// StructuredCounter buffer helper methods.
        /// </summary>
        /// <remarks>
        /// Example in HLSL: StructuredBuffer&lt;float4&gt; or RWStructuredBuffer&lt;float4&gt for structured buffers supporting unordered access.
        /// </remarks>
        public static class StructuredCounter
        {
            /// <summary>
            /// Creates a new StructuredCounter buffer accessible as a <see cref="ShaderResourceView" /> and as a <see cref="UnorderedAccessView" />.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="count">The number of element in this buffer.</param>
            /// <param name="elementSize">Size of the struct.</param>
            /// <returns>A StructuredCounter buffer</returns>
            public static Buffer New(GraphicsDevice device, int count, int elementSize)
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredCounterBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New(device, count * elementSize, elementSize, BufferFlags);
            }

            /// <summary>
            /// Creates a new StructuredCounter buffer accessible as a <see cref="ShaderResourceView" /> and optionaly as a <see cref="UnorderedAccessView" />.
            /// </summary>
            /// <typeparam name="T">Type of the element in the structured buffer</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="count">The number of element in this buffer.</param>
            /// <returns>A Structured buffer</returns>
            public static Buffer<T> New<T>(GraphicsDevice device, int count) where T : struct
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredCounterBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New<T>(device, count, BufferFlags);
            }

            /// <summary>
            /// Creates a new StructuredCounter buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <typeparam name="T">Type of the StructuredCounter buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the StructuredCounter buffer.</param>
            /// <returns>A StructuredCounter buffer</returns>
            public static Buffer New<T>(GraphicsDevice device, T[] value) where T : struct
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredCounterBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New(device, value, BufferFlags);
            }

            /// <summary>
            /// Creates a new StructuredCounter buffer <see cref="GraphicsResourceUsage.Default" /> uasge.
            /// </summary>
            /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
            /// <param name="value">The value to initialize the StructuredCounter buffer.</param>
            /// <param name="elementSize">Size of the element.</param>
            /// <returns>A StructuredCounter buffer</returns>
            public static Buffer New(GraphicsDevice device, DataPointer value, int elementSize)
            {
                const BufferFlags BufferFlags = BufferFlags.StructuredCounterBuffer | BufferFlags.ShaderResource | BufferFlags.UnorderedAccess;
                return Buffer.New(device, value, elementSize, BufferFlags);
            }
        }
    }
}
