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
    /// Defines a type converter for <see cref="Color"/>.
    /// </summary>
    public class ColorConverter : BaseConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorConverter"/> class.
        /// </summary>
        public ColorConverter()
        {
            var type = typeof(Color);
            Properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new FieldPropertyDescriptor(type.GetField(nameof(Color.R))),
                new FieldPropertyDescriptor(type.GetField(nameof(Color.G))),
                new FieldPropertyDescriptor(type.GetField(nameof(Color.B))),
                new FieldPropertyDescriptor(type.GetField(nameof(Color.A))),
            });
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Color3) || destinationType == typeof(Color4) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));

            if (value is Color)
            {
                var color = (Color)value;

                if (destinationType == typeof(string))
                {
                    return color.ToString();
                }
                if (destinationType == typeof(Color3))
                {
                    return color.ToColor3();
                }
                if (destinationType == typeof(Color4))
                {
                    return color.ToColor4();
                }
                if (destinationType == typeof(InstanceDescriptor))
                {
                    var constructor = typeof(Color).GetConstructor(MathUtil.Array(typeof(byte), 4));
                    if (constructor != null)
                        return new InstanceDescriptor(constructor, color.ToArray());
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Color3) || sourceType == typeof(Color4) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is Color3)
            {
                var color = (Color3)value;
                return (Color)color;
            }
            if (value is Color4)
            {
                var color = (Color4)value;
                return (Color)color;
            }

            var str = value as string;
            if (str != null)
            {
                var colorValue = ColorExtensions.StringToRgba(str);
                return Color.FromRgba(colorValue);
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        [NotNull]
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null) throw new ArgumentNullException(nameof(propertyValues));
            return new Color((byte)propertyValues[nameof(Color.R)], (byte)propertyValues[nameof(Color.G)], (byte)propertyValues[nameof(Color.B)], (byte)propertyValues[nameof(Color.A)]);
        }
    }
}
