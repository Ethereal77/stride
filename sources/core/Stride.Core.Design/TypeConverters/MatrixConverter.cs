// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Copyright (c) 2007-2011 SlimDX Group
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

using Xenko.Core.Annotations;
using Xenko.Core.Mathematics;

namespace Xenko.Core.TypeConverters
{
    /// <summary>
    /// Defines a type converter for <see cref="Matrix"/>.
    /// </summary>
    public class MatrixConverter : BaseConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixConverter"/> class.
        /// </summary>
        public MatrixConverter()
        {
            Type type = typeof(Matrix);
            Properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M11))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M12))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M13))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M14))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M21))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M22))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M23))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M24))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M31))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M32))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M33))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M34))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M41))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M42))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M43))),
                new FieldPropertyDescriptor(type.GetField(nameof(Matrix.M44))),
            });
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value is Matrix)
            {
                var matrix = (Matrix)value;

                if (destinationType == typeof(string))
                    return matrix.ToString();

                if (destinationType == typeof(InstanceDescriptor))
                {
                    var constructor = typeof(Matrix).GetConstructor(MathUtil.Array(typeof(float), 16));
                    if (constructor != null)
                        return new InstanceDescriptor(constructor, matrix.ToArray());
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str))
                return null;

            str = str.Replace("[", string.Empty).Replace("]", string.Empty);
            return ConvertFromString<Matrix, float>(context, culture, str);
        }

        /// <inheritdoc/>
        [NotNull]
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null) throw new ArgumentNullException(nameof(propertyValues));
            var matrix = new Matrix
            {
                M11 = (float)propertyValues[nameof(Matrix.M11)],
                M12 = (float)propertyValues[nameof(Matrix.M12)],
                M13 = (float)propertyValues[nameof(Matrix.M13)],
                M14 = (float)propertyValues[nameof(Matrix.M14)],
                M21 = (float)propertyValues[nameof(Matrix.M21)],
                M22 = (float)propertyValues[nameof(Matrix.M22)],
                M23 = (float)propertyValues[nameof(Matrix.M23)],
                M24 = (float)propertyValues[nameof(Matrix.M24)],
                M31 = (float)propertyValues[nameof(Matrix.M31)],
                M32 = (float)propertyValues[nameof(Matrix.M32)],
                M33 = (float)propertyValues[nameof(Matrix.M33)],
                M34 = (float)propertyValues[nameof(Matrix.M34)],
                M41 = (float)propertyValues[nameof(Matrix.M41)],
                M42 = (float)propertyValues[nameof(Matrix.M42)],
                M43 = (float)propertyValues[nameof(Matrix.M43)],
                M44 = (float)propertyValues[nameof(Matrix.M44)],
            };
            return matrix;
        }
    }
}
