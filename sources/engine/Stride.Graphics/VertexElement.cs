// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Core.Serialization;

using Half = Stride.Core.Mathematics.Half;

namespace Stride.Graphics
{
    /// <summary>
    ///   Description of a single element for the input-assembler stage.
    /// </summary>
    /// <remarks>
    ///   This structure is related to <see cref="Direct3D11.InputElement"/>.
    ///   <para/>
    ///   Because <see cref="Direct3D11.InputElement"/> requires to have the same <see cref="VertexBufferLayout.SlotIndex"/>,
    ///   <see cref="VertexBufferLayout.VertexClassification"/> and <see cref="VertexBufferLayout.instanceDataStepRate"/>,
    ///   the <see cref="VertexBufferLayout"/> structure encapsulates a set of <see cref="VertexElement"/> for a particular
    ///   slot, classification and instance data step rate.
    ///   <para/>
    ///   Unlike the default <see cref="Direct3D11.InputElement"/>, this structure accepts a semantic name with a postfix
    ///   number that will be automatically extracted to the semantic index.
    /// </remarks>
    /// <seealso cref="VertexBufferLayout"/>
    [DataContract]
    [DataSerializer(typeof(Serializer))]
    public struct VertexElement : IEquatable<VertexElement>
    {
        private readonly int hashCode;

        // Match the last digit of a semantic name
        internal static readonly Regex MatchSemanticIndex = new Regex(@"(.*)(\d+)$");

        /// <summary>
        ///   Returns a value that can be used for the offset parameter of an InputElement to indicate that the element
        ///   should be aligned directly after the previous element, including any packing if neccessary.
        /// </summary>
        /// <returns>A value used to align input elements.</returns>
        public const int AppendAligned = -1;

        /// <summary>
        ///   Initializes a new instance of the <see cref="VertexElement"/> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <param name="format">The format.</param>
        /// <remarks>
        ///   If the semantic name contains a postfix number, this number will be used as a semantic index.
        /// </remarks>
        public VertexElement(string semanticName, PixelFormat format)
            : this()
        {
            if (semanticName is null)
                throw new ArgumentNullException(nameof(semanticName));

            // All semantics will be upper case
            semanticName = semanticName.ToUpperInvariant();

            var match = MatchSemanticIndex.Match(semanticName);
            if (match.Success)
            {
                // Convert to singleton string in order to speed up things
                SemanticName = match.Groups[1].Value;
                SemanticIndex = int.Parse(match.Groups[2].Value);
            }
            else
            {
                SemanticName = semanticName;
            }

            Format = format;
            AlignedByteOffset = AppendAligned;

            // Precalculate hashcode
            hashCode = ComputeHashCode();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VertexElement"/> struct.
        /// </summary>
        /// <param name="semanticName">Name of the semantic.</param>
        /// <param name="semanticIndex">Index of the semantic.</param>
        /// <param name="format">The format.</param>
        /// <param name="alignedByteOffset">The aligned byte offset.</param>
        public VertexElement(string semanticName, int semanticIndex, PixelFormat format, int alignedByteOffset = AppendAligned)
            : this()
        {
            if (semanticName is null)
                throw new ArgumentNullException(nameof(semanticName));

            // All semantics will be upper case
            semanticName = semanticName.ToUpperInvariant();

            var match = MatchSemanticIndex.Match(semanticName);
            if (match.Success)
                throw new ArgumentException("Semantic name cannot have a semantic index when using the constructor with explicit semantic index. Use implicit semantic index constructor.");

            // Convert to singleton string in order to speed up things
            SemanticName = semanticName;
            SemanticIndex = semanticIndex;
            Format = format;
            AlignedByteOffset = alignedByteOffset;

            // Precalculate hashcode
            hashCode = ComputeHashCode();
        }

        /// <summary>
        ///   Gets the HLSL semantic associated with this element in a shader input-signature.
        /// </summary>
        public string SemanticName { get; private set; }

        /// <summary>
        ///   Gets the HLSL semantic associated with this element in a shader input-signature, including the semantic index (if any).
        /// </summary>
        public string SemanticAsText => SemanticIndex == 0
                    ? SemanticName
                    : $"{SemanticName}{SemanticIndex.ToString(CultureInfo.InvariantCulture)}";

        /// <summary>
        ///   Gets the semantic index for the element.
        /// </summary>
        /// <remarks>
        ///   A semantic index modifies a semantic with an integer index number. It is only needed in a case where there is more than one element with the same semantic.
        ///   <para/>
        ///   For example, a 4x4 matrix would have four components each with the semantic name <c>MATRIX</c>, however each of the four components would have
        ///   different semantic indices (0, 1, 2, and 3).
        /// </remarks>
        public int SemanticIndex { get; private set; }

        /// <summary>
        ///   Gets the data type of the element.
        /// </summary>
        /// <seealso cref="PixelFormat"/>
        public PixelFormat Format { get; private set; }

        /// <summary>
        ///   Gets the offset (in bytes) between each element.
        /// </summary>
        /// <value>
        ///   The byte offset of this element. Specify <see cref="AppendAligned"/> for convenience to define the current element directly after the previous one,
        ///   including any packing if necessary.
        /// </value>
        /// <remarks>
        ///   This value is optional.
        /// </remarks>
        public int AlignedByteOffset { get; private set; }

        public bool Equals(VertexElement other)
        {
            // First use hashCode to compute
            return hashCode == other.hashCode &&
                   SemanticName.Equals(other.SemanticName) &&
                   SemanticIndex == other.SemanticIndex &&
                   Format == other.Format &&
                   AlignedByteOffset == other.AlignedByteOffset;
        }

        public override bool Equals(object obj) => obj is VertexElement vertexElement && Equals(vertexElement);

        public override int GetHashCode() => hashCode;

        internal int ComputeHashCode()
        {
            unchecked
            {
                int localHashCode = SemanticName.GetHashCode();
                localHashCode = (localHashCode * 397) ^ SemanticIndex;
                localHashCode = (localHashCode * 397) ^ Format.GetHashCode();
                localHashCode = (localHashCode * 397) ^ AlignedByteOffset;
                return localHashCode;
            }
        }

        public static bool operator ==(VertexElement left, VertexElement right) => left.Equals(right);

        public static bool operator !=(VertexElement left, VertexElement right) => !left.Equals(right);

        public override string ToString()
        {
            return string.Format("{0}{1},{2},{3}",
                SemanticName,
                SemanticIndex == 0 ? string.Empty : SemanticIndex.ToString(CultureInfo.InvariantCulture),
                Format,
                AlignedByteOffset);
        }


        /// <summary>
        ///   Declares a vertex element with the semantic <c>COLOR</c>.
        /// </summary>
        /// <typeparam name="T">Type of the Color semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Color<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return Color(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>COLOR</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Color(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return Color(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>COLOR</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Color(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("COLOR", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>NORMAL</c>.
        /// </summary>
        /// <typeparam name="T">Type of the Normal semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Normal<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return Normal(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>NORMAL</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Normal(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return Normal(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>NORMAL</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Normal(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("NORMAL", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>POSITION</c>.
        /// </summary>
        /// <typeparam name="T">Type of the Position semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Position<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return Position(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>POSITION</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Position(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return Position(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>POSITION</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Position(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("POSITION", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>SV_POSITION</c>.
        /// </summary>
        /// <typeparam name="T">Type of the PositionTransformed semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement PositionTransformed<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return PositionTransformed(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>SV_POSITION</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement PositionTransformed(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return PositionTransformed(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>SV_POSITION</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement PositionTransformed(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("SV_POSITION", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>TEXCOORD</c>.
        /// </summary>
        /// <typeparam name="T">Type of the TextureCoordinate semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement TextureCoordinate<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return TextureCoordinate(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>TEXCOORD</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement TextureCoordinate(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return TextureCoordinate(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>TEXCOORD</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement TextureCoordinate(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("TEXCOORD", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>TANGENT</c>.
        /// </summary>
        /// <typeparam name="T">Type of the Tangent semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Tangent<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return Tangent(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>TANGENT</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Tangent(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return Tangent(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>TANGENT</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement Tangent(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("TANGENT", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>BITANGENT</c>.
        /// </summary>
        /// <typeparam name="T">Type of the BiTangent semantic.</typeparam>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement BiTangent<T>(int semanticIndex = 0, int offsetInBytes = AppendAligned) where T : struct
        {
            return BiTangent(semanticIndex, ConvertTypeToFormat<T>(), offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>BITANGENT</c>.
        /// </summary>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement BiTangent(PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return BiTangent(0, format, offsetInBytes);
        }

        /// <summary>
        ///   Declares a vertex element with the semantic <c>BITANGENT</c>.
        /// </summary>
        /// <param name="semanticIndex">The semantic index.</param>
        /// <param name="format">Format of this element.</param>
        /// <param name="offsetInBytes">The offset in bytes of this element. Use <see cref="AppendAligned"/> to compute automatically the offset from previous elements.</param>
        /// <returns>A new <see cref="VertexElement"/> that represents this semantic.</returns>
        public static VertexElement BiTangent(int semanticIndex, PixelFormat format, int offsetInBytes = AppendAligned)
        {
            return new VertexElement("BITANGENT", semanticIndex, format, offsetInBytes);
        }

        /// <summary>
        ///   Converts a type to a <see cref="PixelFormat"/> to use as data format for a vertex element.
        /// </summary>
        /// <typeparam name="T">The type of the vertex element.</typeparam>
        /// <returns>The equivalent <see cref="PixelFormat"/>.</returns>
        /// <exception cref="NotSupportedException">The convertion for this type is not supported.</exception>
        public static PixelFormat ConvertTypeToFormat<T>() where T : struct
        {
            return ConvertTypeToFormat(typeof(T));
        }

        /// <summary>
        ///   Converts a type to a <see cref="PixelFormat"/> to use as data format for a vertex element.
        /// </summary>
        /// <param name="type">The type of the vertex element.</param>
        /// <returns>The equivalent <see cref="PixelFormat"/>.</returns>
        /// <exception cref="NotSupportedException">The convertion for this type is not supported.</exception>
        private static PixelFormat ConvertTypeToFormat(Type type)
        {
            if (typeof(Vector4) == type || typeof(Color4) == type)
                return PixelFormat.R32G32B32A32_Float;
            if (typeof(Vector3) == type || typeof(Color3) == type)
                return PixelFormat.R32G32B32_Float;
            if (typeof(Vector2) == type)
                return PixelFormat.R32G32_Float;
            if (typeof(float) == type)
                return PixelFormat.R32_Float;

            if (typeof(Color) == type)
                return PixelFormat.R8G8B8A8_UNorm;
            if (typeof(ColorBGRA) == type)
                return PixelFormat.B8G8R8A8_UNorm;

            if (typeof(Half4) == type)
                return PixelFormat.R16G16B16A16_Float;
            if (typeof(Half2) == type)
                return PixelFormat.R16G16_Float;
            if (typeof(Half) == type)
                return PixelFormat.R16_Float;

            if (typeof(Int4) == type)
                return PixelFormat.R32G32B32A32_UInt;
            if (typeof(Int3) == type)
                return PixelFormat.R32G32B32_UInt;
            if (typeof(int) == type)
                return PixelFormat.R32_UInt;
            if (typeof(uint) == type)
                return PixelFormat.R32_UInt;

            //if (typeof(Bool4) == typeT)
            //    return PixelFormat.R32G32B32A32_UInt;

            //if (typeof(Bool) == typeT)
            //    return PixelFormat.R32_UInt;

            throw new NotSupportedException($"Type [{type.Name}] is not supported. You must specify an explicit DXGI Format.");
        }

        internal class Serializer : DataSerializer<VertexElement>
        {
            public override void Serialize(ref VertexElement obj, ArchiveMode mode, SerializationStream stream)
            {
                if (mode == ArchiveMode.Deserialize)
                {
                    obj.SemanticName = stream.ReadString();
                    obj.SemanticIndex = stream.ReadInt32();
                    obj.Format = stream.Read<PixelFormat>();
                    obj.AlignedByteOffset = stream.ReadInt32();

                    obj.ComputeHashCode();
                }
                else
                {
                    stream.Write(obj.SemanticName);
                    stream.Write(obj.SemanticIndex);
                    stream.Write(obj.Format);
                    stream.Write(obj.AlignedByteOffset);
                }
            }
        }
    }
}
