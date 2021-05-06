// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Serialization;

namespace Stride.Graphics
{
    /// <summary>
    ///   Binding structure that specifies a Vertex <see cref="Graphics.Buffer"/> and other per-vertex parameters
    ///   (such as offset and instancing) for a graphics device.
    /// </summary>
    [DataSerializer(typeof(VertexBufferBinding.Serializer))]
    public struct VertexBufferBinding : IEquatable<VertexBufferBinding>
    {
        private readonly int hashCode;

        /// <summary>
        ///   Initializes a new instance of the <see cref="VertexBufferBinding"/> struct.
        /// </summary>
        /// <param name="vertexBuffer">The buffer containing the vertices.</param>
        /// <param name="vertexDeclaration">A <see cref="VertexDeclaration"/> specifying tha layout of the vertices.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="vertexStride">
        ///   Number of bytes to advance to get to the next vertex.
        ///   If -1, it gets auto-discovered from the <paramref name="vertexDeclaration"/>.
        /// </param>
        /// <param name="vertexOffset">
        ///   Offset (in Vertex ElementCount) from the beginning of the buffer to the first vertex to use.
        /// </param>
        public VertexBufferBinding(Buffer vertexBuffer, VertexDeclaration vertexDeclaration, int vertexCount, int vertexStride = -1, int vertexOffset = 0) : this()
        {
            Buffer = vertexBuffer ?? throw new ArgumentNullException(nameof(vertexBuffer));
            Declaration = vertexDeclaration ?? throw new ArgumentNullException(nameof(vertexDeclaration));

            Stride = vertexStride != -1 ? vertexStride : vertexDeclaration.VertexStride;
            Offset = vertexOffset;
            Count = vertexCount;

            HashCode hash = default;
            hash.Add(Buffer.GetHashCode());
            hash.Add(Offset);
            hash.Add(Stride);
            hash.Add(Count);
            hash.Add(Declaration.GetHashCode());
            hashCode = hash.ToHashCode();
        }


        /// <summary>
        ///   Gets the Vertex Buffer.
        /// </summary>
        public Buffer Buffer { get; private set; }

        /// <summary>
        ///   Gets the offset (vertex index) between the beginning of the buffer and the vertex data to use.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        ///   Gets the vertex stride, the number of bytes a single vertex occupies in the buffer.
        /// </summary>
        public int Stride { get; private set; }

        /// <summary>
        ///   Gets the number of vertices.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///   Gets the layout of the vertices in the Vertex Buffer.
        /// </summary>
        public VertexDeclaration Declaration { get; private set; }


        /// <inheritdoc/>
        public bool Equals(VertexBufferBinding other) => Buffer.Equals(other.Buffer) &&
                                                         Offset == other.Offset &&
                                                         Stride == other.Stride &&
                                                         Count == other.Count &&
                                                         Declaration.Equals(other.Declaration);

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            return obj is VertexBufferBinding vertexBufferBinding && Equals(vertexBufferBinding);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => hashCode;


        //
        // Data serializer for Vertex Buffer bindings.
        //
        internal class Serializer : DataSerializer<VertexBufferBinding>
        {
            public override void Serialize(ref VertexBufferBinding vertexBufferBinding, ArchiveMode mode, SerializationStream stream)
            {
                if (mode == ArchiveMode.Deserialize)
                {
                    var buffer = stream.Read<Buffer>();
                    var declaration = stream.Read<VertexDeclaration>();
                    var count = stream.ReadInt32();
                    var stride = stream.ReadInt32();
                    var offset = stream.ReadInt32();

                    vertexBufferBinding = new VertexBufferBinding(buffer, declaration, count, stride, offset);
                }
                else
                {
                    stream.Write(vertexBufferBinding.Buffer);
                    stream.Write(vertexBufferBinding.Declaration);
                    stream.Write(vertexBufferBinding.Count);
                    stream.Write(vertexBufferBinding.Stride);
                    stream.Write(vertexBufferBinding.Offset);
                }
            }
        }
    }
}
