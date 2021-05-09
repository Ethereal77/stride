// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Serialization;

namespace Stride.Graphics
{
    /// <summary>
    ///   Binding structure that specifies an Index <see cref="Graphics.Buffer"/> and other per-index parameters
    ///   (such as offset and format) for a graphics device.
    /// </summary>
    [DataSerializer(typeof(IndexBufferBinding.Serializer))]
    public class IndexBufferBinding
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="IndexBufferBinding"/> class.
        /// </summary>
        /// <param name="indexBuffer">The buffer containing the indices.</param>
        /// <param name="is32Bit">
        ///   A value indicating whether each vertex is a 32-bit integer (<c>true</c>),
        ///   or a 16-bit integer (<c>false</c>).
        /// </param>
        /// <param name="indexCount">The number of indices.</param>
        /// <param name="indexOffset">
        ///   Offset (in number of indices) from the beginning of the buffer to the first index to use.
        /// </param>
        public IndexBufferBinding(Buffer indexBuffer, bool is32Bit, int indexCount, int indexOffset = 0)
        {
            Buffer = indexBuffer ?? throw new ArgumentNullException(nameof(indexBuffer));

            Is32Bit = is32Bit;
            Offset = indexOffset;
            Count = indexCount;
        }


        /// <summary>
        ///   Gets the Index Buffer.
        /// </summary>
        public Buffer Buffer { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether the indices are 32-bit.
        /// </summary>
        /// <value><c>true</c> if the indices are 32-bit; <c>false</c> if the indices are 16-bit.</value>
        public bool Is32Bit { get; private set; }

        /// <summary>
        ///   Gets the offset (index element) between the beginning of the buffer and the index data to use.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        ///   Gets the number of indices.
        /// </summary>
        public int Count { get; private set; }


        //
        // Data serializer for Index Buffer bindings.
        //
        internal class Serializer : DataSerializer<IndexBufferBinding>
        {
            public override void Serialize(ref IndexBufferBinding indexBufferBinding, ArchiveMode mode, SerializationStream stream)
            {
                if (mode == ArchiveMode.Deserialize)
                {
                    var buffer = stream.Read<Buffer>();
                    var is32Bit = stream.ReadBoolean();
                    var count = stream.ReadInt32();
                    var offset = stream.ReadInt32();

                    indexBufferBinding = new IndexBufferBinding(buffer, is32Bit, count, offset);
                }
                else
                {
                    stream.Write(indexBufferBinding.Buffer);
                    stream.Write(indexBufferBinding.Is32Bit);
                    stream.Write(indexBufferBinding.Count);
                    stream.Write(indexBufferBinding.Offset);
                }
            }
        }
    }
}
