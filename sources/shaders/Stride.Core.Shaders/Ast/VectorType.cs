// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    ///   Base class for all vector types.
    /// </summary>
    public partial class VectorType : GenericBaseType
    {
        #region Constants and Fields

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a two-component signed integer vector.
        /// </summary>
        public static readonly VectorType Int2 = new VectorType(ScalarType.Int, 2);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a three-component signed integer vector.
        /// </summary>
        public static readonly VectorType Int3 = new VectorType(ScalarType.Int, 3);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a four-component signed integer vector.
        /// </summary>
        public static readonly VectorType Int4 = new VectorType(ScalarType.Int, 4);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a two-component unsigned integer vector.
        /// </summary>
        public static readonly VectorType UInt2 = new VectorType(ScalarType.UInt, 2);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a three-component unsigned integer vector.
        /// </summary>
        public static readonly VectorType UInt3 = new VectorType(ScalarType.UInt, 3);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a four-component unsigned integer vector.
        /// </summary>
        public static readonly VectorType UInt4 = new VectorType(ScalarType.UInt, 4);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a two-component single precision (32-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Float2 = new VectorType(ScalarType.Float, 2);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a three-component single precision (32-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Float3 = new VectorType(ScalarType.Float, 3);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a four-component single precision (32-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Float4 = new VectorType(ScalarType.Float, 4);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a two-component double precision (64-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Double2 = new VectorType(ScalarType.Double, 2);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a three-component double precision (64-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Double3 = new VectorType(ScalarType.Double, 3);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a four-component double precision (64-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Double4 = new VectorType(ScalarType.Double, 4);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a two-component half precision (16-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Half2 = new VectorType(ScalarType.Half, 2);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a three-component half precision (16-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Half3 = new VectorType(ScalarType.Half, 3);

        /// <summary>
        ///   The <see cref="VectorType"/> that represents a four-component half precision (16-bit) floating point vector.
        /// </summary>
        public static readonly VectorType Half4 = new VectorType(ScalarType.Half, 4);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "VectorType" /> class.
        /// </summary>
        public VectorType()
            : base("vector", 2)
        {
            ParameterTypes.Add(typeof(TypeBase));
            ParameterTypes.Add(typeof(Literal));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="VectorType"/> class.
        /// </summary>
        /// <param name="type">The scalar type of the vector's components.</param>
        /// <param name="dimension">The dimensions of the vector. How many components it has.</param>
        public VectorType(ScalarType type, int dimension) : this()
        {
            Type = type;
            Dimension = dimension;
        }


        public override TypeBase ToNonGenericType(SourceSpan? span = null)
        {
            var typeName = new TypeName();
            var name = string.Format("{0}{1}", Type.Name, Dimension);
            typeName.Name = new Identifier(name);
            if (span.HasValue)
            {
                typeName.Span = span.Value;
                typeName.Name.Span = span.Value;
            };
            typeName.TypeInference.TargetType = this;
            return typeName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the dimension of this vector type.
        /// </summary>
        /// <value>The dimension of this vector type.</value>
        public int Dimension
        {
            get => (int) ((Literal)Parameters[1]).Value;
            set => Parameters[1] = new Literal(value);
        }

        /// <summary>
        ///   Gets or sets the scalar type of this vector type's components.
        /// </summary>
        /// <value>The scalar type of this vector type's components.</value>
        public TypeBase Type
        {
            get => (TypeBase)Parameters[0];
            set => Parameters[0] = value;
        }

        #endregion

        /// <inheritdoc/>
        public bool Equals(VectorType other)
        {
            return base.Equals(other);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is VectorType vectorType)
                return Equals(vectorType);

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(VectorType left, VectorType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(VectorType left, VectorType right)
        {
            return !Equals(left, right);
        }
    }
}
