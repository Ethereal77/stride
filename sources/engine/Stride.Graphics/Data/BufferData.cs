// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Core.Serialization.Contents;

namespace Stride.Graphics.Data
{
    /// <summary>
    ///   Content of a GPU buffer (vertex buffer, index buffer, etc).
    /// </summary>
    [DataContract]
    [ContentSerializer(typeof(DataContentSerializerWithReuse<BufferData>))]
    public class BufferData
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="BufferData"/> class.
        /// </summary>
        public BufferData() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BufferData"/> class.
        /// </summary>
        /// <param name="bufferFlags"></param>
        /// <param name="content"></param>
        public BufferData(BufferFlags bufferFlags, byte[] content)
        {
            Content = content;
            BufferFlags = bufferFlags;
        }

        /// <summary>
        ///   Gets or sets the buffer content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        ///   Gets or sets the buffer flags describing the type of buffer.
        /// </summary>
        public BufferFlags BufferFlags { get; set; }

        /// <summary>
        ///   Gets or sets the usage of this buffer.
        /// </summary>
        public GraphicsResourceUsage Usage { get; set; }

        /// <summary>
        ///   Gets or sets the size of the structure when it represents a <see cref="BufferFlags.StructuredBuffer"/>, in bytes.
        /// </summary>
        public int StructureByteStride { get; set; }


        /// <summary>
        ///   Creates a new instance of <see cref="BufferData"/> from a typed buffer.
        /// </summary>
        /// <typeparam name="T">Type of the elements to store in the buffer.</typeparam>
        /// <param name="bufferFlags">The flags indicating the type of buffer.</param>
        /// <param name="content">An array of data.</param>
        /// <returns>The buffer data.</returns>
        public static BufferData New<T>(BufferFlags bufferFlags, T[] content) where T : struct
        {
            var sizeOf = Utilities.SizeOf(content);
            var buffer = new byte[sizeOf];
            Utilities.Write(buffer, content, 0, content.Length);

            return new BufferData(bufferFlags, buffer);
        }
    }
}
