// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Stride.Core.Mathematics;

namespace Stride.Graphics
{
    /// <summary>
    ///   Describes a vertex format structure that contains position, normal and texture coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VertexPositionNormalTexture : IEquatable<VertexPositionNormalTexture>, IVertex
    {
        /// <summary>
        ///   Position of the vertex.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Normal vector of the vertex.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        ///   UV texture coordinates.
        /// </summary>
        public Vector2 TextureCoordinate;


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
        /// Defines structure byte size.
        /// </summary>
        public static readonly int Size = 32;

        /// <summary>
        ///   The vertex layout of this struct.
        /// </summary>
        public static readonly VertexDeclaration Layout = new VertexDeclaration(
            VertexElement.Position<Vector3>(),
            VertexElement.Normal<Vector3>(),
            VertexElement.TextureCoordinate<Vector2>());

        public VertexDeclaration GetLayout() => Layout;

        public bool Equals(VertexPositionNormalTexture other)
        {
            return Position.Equals(other.Position) &&
                   Normal.Equals(other.Normal) &&
                   TextureCoordinate.Equals(other.TextureCoordinate);
        }

        public override bool Equals(object obj)
        {
            return obj is VertexPositionNormalTexture vertex && Equals(vertex);
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

        public override string ToString() => $"Position: {Position}, Normal: {Normal}, Texcoord: {TextureCoordinate}";
    }
}
