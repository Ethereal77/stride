// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.InteropServices;

using Xenko.Core.Mathematics;

namespace Xenko.Graphics
{
    /// <summary>
    /// Describes a custom vertex format structure that contains position, normal and texture information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VertexPositionNormalTexture : IEquatable<VertexPositionNormalTexture>, IVertex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionNormalTexture"/> struct.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="normal">The vertex normal.</param>
        /// <param name="textureCoordinate">UV texture coordinates.</param>
        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate) : this()
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }

        /// <summary>
        /// XYZ position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The vertex normal.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// UV texture coordinates.
        /// </summary>
        public Vector2 TextureCoordinate;
        
        /// <summary>
        /// Defines structure byte size.
        /// </summary>
        public static readonly int Size = 32;

        /// <summary>
        /// The vertex layout of this struct.
        /// </summary>
        public static readonly VertexDeclaration Layout = new VertexDeclaration(
            VertexElement.Position<Vector3>(),
            VertexElement.Normal<Vector3>(),
            VertexElement.TextureCoordinate<Vector2>());

        public bool Equals(VertexPositionNormalTexture other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureCoordinate.Equals(other.TextureCoordinate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTexture && Equals((VertexPositionNormalTexture)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ TextureCoordinate.GetHashCode();
                return hashCode;
            }
        }

        public VertexDeclaration GetLayout()
        {
            return Layout;
        }

        public void FlipWinding()
        {
            TextureCoordinate.X = (1.0f - TextureCoordinate.X);
        }

        public static bool operator ==(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Normal: {1}, Texcoord: {2}", Position, Normal, TextureCoordinate);
        }
    }
}
