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
    /// Defines a type converter for <see cref="Quaternion"/>.
    /// </summary>
    public class QuaternionConverter : BaseConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionConverter"/> class.
        /// </summary>
        public QuaternionConverter()
        {
            var type = typeof(Quaternion);
            Properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new FieldPropertyDescriptor(type.GetField(nameof(Quaternion.X))),
                new FieldPropertyDescriptor(type.GetField(nameof(Quaternion.Y))),
                new FieldPropertyDescriptor(type.GetField(nameof(Quaternion.Z))),
                new FieldPropertyDescriptor(type.GetField(nameof(Quaternion.W))),
            });
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value is Quaternion)
            {
                var quaternion = (Quaternion)value;

                if (destinationType == typeof(string))
                    return quaternion.ToString();

                if (destinationType == typeof(InstanceDescriptor))
                {
                    var constructor = typeof(Quaternion).GetConstructor(MathUtil.Array(typeof(float), 4));
                    if (constructor != null)
                        return new InstanceDescriptor(constructor, quaternion.ToArray());
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value != null ? ConvertFromString<Quaternion, float>(context, culture, value) : base.ConvertFrom(context, culture, null);
        }

        /// <inheritdoc/>
        [NotNull]
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null) throw new ArgumentNullException(nameof(propertyValues));
            return new Quaternion((float)propertyValues[nameof(Quaternion.X)], (float)propertyValues[nameof(Quaternion.Y)], (float)propertyValues[nameof(Quaternion.Z)], (float)propertyValues[nameof(Quaternion.W)]);
        }
    }
}
