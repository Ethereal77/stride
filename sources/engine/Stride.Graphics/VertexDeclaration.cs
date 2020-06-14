// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core;
using Stride.Core.Serialization;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents the <see cref="VertexElement"/>s that define the layout of a vertex in a vertex buffer.
    /// </summary>
    [DataContract]
    [DataSerializer(typeof(Serializer))]
    public class VertexDeclaration : IEquatable<VertexDeclaration>
    {
        private readonly VertexElement[] elements;
        private readonly int instanceCount;
        private readonly int vertexStride;
        private readonly int hashCode;

        /// <summary>
        ///   Initializes a new instance of the <see cref="VertexDeclaration"/> class.
        /// </summary>
        internal VertexDeclaration() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VertexDeclaration"/> class.
        /// </summary>
        /// <param name="elements">The elements that compose a vertex.</param>
        public VertexDeclaration(params VertexElement[] elements)
            : this(elements, 0, 0)
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VertexDeclaration"/> class.
        /// </summary>
        /// <param name="elements">The elements that compose a vertex.</param>
        /// <param name="instanceCount">The instance count.</param>
        /// <param name="vertexStride">The vertex stride, in bytes. Specify 0 to compute the stride from <paramref name="elements"/>.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="elements"/> is a <c>null</c> reference.</exception>
        public VertexDeclaration(VertexElement[] elements, int instanceCount, int vertexStride)
        {
            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            this.elements = elements;
            this.vertexStride = vertexStride == 0 ? VertexElementValidator.GetVertexStride(elements) : vertexStride;
            this.instanceCount = instanceCount;

            // Validate vertices
            VertexElementValidator.Validate(vertexStride, elements);

            hashCode = instanceCount;
            hashCode = (hashCode * 397) ^ vertexStride;
            foreach (var vertexElement in elements)
            {
                hashCode = (hashCode * 397) ^ vertexElement.GetHashCode();
            }
        }

        /// <summary>
        ///   Gets the vertex elements.
        /// </summary>
        /// <value>An array of the <see cref="VertexElement"/> that compose a vertex.</value>
        public VertexElement[] VertexElements => elements;

        /// <summary>
        ///   Gets the instance count.
        /// </summary>
        /// <value>The instance count.</value>
        public int InstanceCount => instanceCount;

        /// <summary>
        ///   Gets the vertex stride.
        /// </summary>
        /// <value>The vertex stride, in bytes.</value>
        public int VertexStride => vertexStride;

        /// <summary>
        ///   Enumerates <see cref="VertexElement"/> with declared offsets.
        /// </summary>
        /// <returns>A collection of the <see cref="VertexElement"/>s with their respective offsets.</returns>
        public IEnumerable<VertexElementWithOffset> EnumerateWithOffsets()
        {
            int offset = 0;
            foreach (var element in VertexElements)
            {
                // Get new offset (if specified)
                var currentElementOffset = element.AlignedByteOffset;
                if (currentElementOffset != VertexElement.AppendAligned)
                    offset = currentElementOffset;

                var elementSize = element.Format.SizeInBytes();
                yield return new VertexElementWithOffset(element, offset, elementSize);

                // Compute next offset (if automatic)
                offset += elementSize;
            }
        }

        /// <summary>
        ///   Calculates the size of this vertex declaration.
        /// </summary>
        /// <returns>The size of this vertex declaration, in bytes.</returns>
        public int CalculateSize()
        {
            var size = 0;
            var offset = 0;
            foreach (var element in VertexElements)
            {
                // Get new offset (if specified)
                var currentElementOffset = element.AlignedByteOffset;
                if (currentElementOffset != VertexElement.AppendAligned)
                    offset = currentElementOffset;

                var elementSize = element.Format.SizeInBytes();

                // Compute next offset (if automatic)
                offset += elementSize;

                // Elements are not necessary ordered by increasing offsets
                size = Math.Max(size, offset);
            }

            return size;
        }

        public bool Equals(VertexDeclaration other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return hashCode == other.hashCode &&
                   vertexStride == other.vertexStride &&
                   instanceCount == other.instanceCount &&
                   Utilities.Compare(elements, other.elements);
        }

        public override bool Equals(object obj)
        {
            return (obj is VertexDeclaration vertexDeclaration) && Equals(vertexDeclaration);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref="VertexElement"/> to <see cref="VertexDeclaration"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The resulting <see cref="VertexDeclaration"/>.</returns>
        public static implicit operator VertexDeclaration(VertexElement element) => new VertexDeclaration(element);

        /// <summary>
        ///   Performs an implicit conversion from <see cref="VertexElement"/>[] to <see cref="VertexDeclaration"/>.
        /// </summary>
        /// <param name="elements">The array of elements.</param>
        /// <returns>The resulting <see cref="VertexDeclaration"/>.</returns>
        public static implicit operator VertexDeclaration(VertexElement[] elements) => new VertexDeclaration(elements);

        internal class Serializer : DataSerializer<VertexDeclaration>, IDataSerializerGenericInstantiation
        {
            public override void PreSerialize(ref object obj, ArchiveMode mode, SerializationStream stream)
            {
                // We are creating object at deserialization time
                if (mode == ArchiveMode.Serialize)
                {
                    base.PreSerialize(ref obj, mode, stream);
                }
            }

            public override void Serialize(ref VertexDeclaration obj, ArchiveMode mode, SerializationStream stream)
            {
                if (mode == ArchiveMode.Deserialize)
                {
                    var elements = stream.Read<VertexElement[]>();
                    var instanceCount = stream.ReadInt32();
                    var vertexStride = stream.ReadInt32();
                    obj = new VertexDeclaration(elements, instanceCount, vertexStride);
                }
                else
                {
                    stream.Write(obj.elements);
                    stream.Write(obj.instanceCount);
                    stream.Write(obj.vertexStride);
                }
            }

            public void EnumerateGenericInstantiations(SerializerSelector serializerSelector, IList<Type> genericInstantiations)
            {
                genericInstantiations.Add(typeof(VertexElement[]));
            }
        }
    }
}
