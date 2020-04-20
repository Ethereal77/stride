// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Copyright (c) 2007-2011 SlimDX Group
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

using Stride.Core.Annotations;
using Stride.Core.Mathematics;

namespace Stride.Core.TypeConverters
{
    /// <summary>
    /// Defines a type converter for <see cref="Vector2"/>.
    /// </summary>
    public class Vector2Converter : BaseConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2Converter"/> class.
        /// </summary>
        public Vector2Converter()
        {
            var type = typeof(Vector2);
            Properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new FieldPropertyDescriptor(type.GetField(nameof(Vector2.X))),
                new FieldPropertyDescriptor(type.GetField(nameof(Vector2.Y))),
            });
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value is Vector2)
            {
                var vector = (Vector2)value;

                if (destinationType == typeof(string))
                    return vector.ToString();

                if (destinationType == typeof(InstanceDescriptor))
                {
                    var constructor = typeof(Vector2).GetConstructor(MathUtil.Array(typeof(float), 2));
                    if (constructor != null)
                        return new InstanceDescriptor(constructor, vector.ToArray());
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value != null ? ConvertFromString<Vector2, float>(context, culture, value) : base.ConvertFrom(context, culture, null);
        }

        /// <inheritdoc/>
        [NotNull]
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null) throw new ArgumentNullException(nameof(propertyValues));
            return new Vector2((float)propertyValues[nameof(Vector2.X)], (float)propertyValues[nameof(Vector2.Y)]);
        }
    }
}
