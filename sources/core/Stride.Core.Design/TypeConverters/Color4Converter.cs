// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Copyright (c) 2007-2011 SlimDX Group
// See the LICENSE.md file in the project root for full license information.

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
    /// Defines a type converter for <see cref="Color4"/>.
    /// </summary>
    public class Color4Converter : BaseConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Color4Converter"/> class.
        /// </summary>
        public Color4Converter()
        {
            var type = typeof(Color4);
            Properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new FieldPropertyDescriptor(type.GetField(nameof(Color4.R))),
                new FieldPropertyDescriptor(type.GetField(nameof(Color4.G))),
                new FieldPropertyDescriptor(type.GetField(nameof(Color4.B))),
                new FieldPropertyDescriptor(type.GetField(nameof(Color4.A))),
            });
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Color) || destinationType == typeof(Color3) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value is Color4)
            {
                var color = (Color4)value;

                if (destinationType == typeof(string))
                {
                    return color.ToString();
                }
                if (destinationType == typeof(Color))
                {
                    return (Color)color;
                }
                if (destinationType == typeof(Color3))
                {
                    return color.ToColor3();
                }

                if (destinationType == typeof(InstanceDescriptor))
                {
                    var constructor = typeof(Color4).GetConstructor(MathUtil.Array(typeof(float), 4));
                    if (constructor != null)
                        return new InstanceDescriptor(constructor, color.ToArray());
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Color) || sourceType == typeof(Color3) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is Color)
            {
                var color = (Color)value;
                return color.ToColor4();
            }
            if (value is Color3)
            {
                var color = (Color3)value;
                return color.ToColor4();
            }

            var str = value as string;
            if (str != null)
            {
                // First try to convert using StringToRgba
                if (ColorExtensions.CanConvertStringToRgba(str))
                {
                    var colorValue = ColorExtensions.StringToRgba(str);
                    return new Color4(colorValue);
                }
                // If we can't, use the default ConvertFromString method.
                return ConvertFromString<Color4, float>(context, culture, value);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        [NotNull]
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null) throw new ArgumentNullException(nameof(propertyValues));
            return new Color4((float)propertyValues[nameof(Color.R)], (float)propertyValues[nameof(Color.G)], (float)propertyValues[nameof(Color.B)], (float)propertyValues[nameof(Color.A)]);
        }
    }
}
