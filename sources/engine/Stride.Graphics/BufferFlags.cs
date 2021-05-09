// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Defines flags to identify how to bind a <see cref="Buffer"/> to the pipeline.
    /// </summary>
    [Flags]
    [DataContract]
    public enum BufferFlags
    {
        /// <summary>
        ///   Creates a buffer without specifying its usage.
        /// </summary>
        None = 0,

        /// <summary>
        ///   Creates a buffer to contain constants for shaders.
        /// </summary>
        ConstantBuffer = 1,

        /// <summary>
        ///   Creates a buffer to contain vertex indices.
        /// </summary>
        IndexBuffer = 2,

        /// <summary>
        ///   Creates a buffer to contain vertices.
        /// </summary>
        VertexBuffer = 4,

        /// <summary>
        ///   Creates a buffer to be used as a render target.
        /// </summary>
        RenderTarget = 8,

        /// <summary>
        ///   Creates a buffer that can be used as a resource for shaders (ShaderResourceView).
        /// </summary>
        ShaderResource = 16,

        /// <summary>
        ///   Creates a buffer that can be accessed in an unordered manner in shaders.
        /// </summary>
        UnorderedAccess = 32,

        /// <summary>
        ///   Creates a structured buffer that can be used in shaders.
        /// </summary>
        StructuredBuffer = 64,

        /// <summary>
        ///   Creates a <see cref="StructuredBuffer"/> that supports unordered acccess and appending data in shaders.
        /// </summary>
        StructuredAppendBuffer = UnorderedAccess | StructuredBuffer | 128,

        /// <summary>
        ///   Creates a structured buffer that supports unordered acccess and counting in shaders.
        /// </summary>
        StructuredCounterBuffer = UnorderedAccess | StructuredBuffer | 256,

        /// <summary>
        ///   Creates a raw buffer.
        /// </summary>
        RawBuffer = 512,

        /// <summary>
        ///   Creates an indirect arguments buffer that can be used for GPU-based dispatch.
        /// </summary>
        ArgumentBuffer = 1024,

        /// <summary>
        ///   Creates a buffer for the geometry shader stream-output stage to write data in.
        /// </summary>
        StreamOutput = 2048
    }
}
