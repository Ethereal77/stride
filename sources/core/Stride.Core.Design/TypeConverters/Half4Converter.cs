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
using System.Reflection;

using Stride.Core.Annotations;
using Stride.Core.Mathematics;

using Half = Stride.Core.Mathematics.Half;

namespace Stride.Core.TypeConverters
{
    /// <summary>
    ///   Provides a type converter to convert <see cref="Half4" /> objects to and from various
    ///   other representations.
    /// </summary>
    public class Half4Converter : ExpandableObjectConverter
    {
        private readonly PropertyDescriptorCollection properties;

        /// <summary>
        ///   Initializes a new instance of the <see cref="Half4Converter"/> class.
        /// </summary>
        public Half4Converter()
        {
            Type type = typeof(Half4);

            properties = new PropertyDescriptorCollection(new PropertyDescriptor[]
                {
                    new FieldPropertyDescriptor(type.GetField("X")),
                    new FieldPropertyDescriptor(type.GetField("Y")),
                    new FieldPropertyDescriptor(type.GetField("Z")),
                    new FieldPropertyDescriptor(type.GetField("W"))
                });
        }

        /// <summary>
        ///   Determines whether this converter can convert an object of the given type to the type of this converter,
        ///   using the specified context.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns>
        ///   <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        /// <summary>
        ///   Determines whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="Type" /> that represents the type you want to convert to.</param>
        /// <returns>
        ///   <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType != typeof(string) &&
                destinationType != typeof(InstanceDescriptor))
            {
                return base.CanConvertTo(context, destinationType);
            }
            return true;
        }

        /// <summary>
        ///   Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="CultureInfo" />. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="object" /> to convert.</param>
        /// <returns>An <see cref="object" /> that represents the converted value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (culture is null)
                culture = CultureInfo.CurrentCulture;

            if (!(value is string @string))
                return base.ConvertFrom(context, culture, value);

            @string = @string.Trim();
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Half));
            char[] separator = new[] { culture.TextInfo.ListSeparator[0] };
            string[] stringArray = @string.Split(separator);
            if (stringArray.Length != 4)
            {
                throw new ArgumentException("Invalid half format.");
            }
            Half x = (Half) converter.ConvertFromString(context, culture, stringArray[0]);
            Half y = (Half) converter.ConvertFromString(context, culture, stringArray[1]);
            Half z = (Half) converter.ConvertFromString(context, culture, stringArray[2]);
            Half w = (Half) converter.ConvertFromString(context, culture, stringArray[3]);
            return new Half4(x, y, z, w);
        }

        /// <summary>
        ///   Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="CultureInfo" />. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="object" /> to convert.</param>
        /// <param name="destinationType">A <see cref="Type" /> that represents the type you want to convert to.</param>
        /// <returns>An <see cref="object" /> that represents the converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType is null)
                throw new ArgumentNullException(nameof(destinationType));

            if (culture is null)
                culture = CultureInfo.CurrentCulture;

            if (destinationType == typeof(string) && value is Half4 half)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Half));
                return string.Join(culture.TextInfo.ListSeparator + " ", new[]
                    {
                        converter.ConvertToString(context, culture, half.X),
                        converter.ConvertToString(context, culture, half.Y),
                        converter.ConvertToString(context, culture, half.Z),
                        converter.ConvertToString(context, culture, half.W)
                    });
            }
            if (destinationType == typeof(InstanceDescriptor) && value is Half4 half2)
            {
                ConstructorInfo info = typeof(Half4).GetConstructor(new[] { typeof(Half), typeof(Half), typeof(Half), typeof(Half) });
                if (info != null)
                    return new InstanceDescriptor(info, new object[] { half2.X, half2.Y, half2.Z, half2.W });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        ///   Creates an instance of the type that this <see cref="TypeConverter" /> is associated with, using the specified
        ///   context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="propertyValues">An <see cref="IDictionary" /> of new property values.</param>
        /// <returns>
        ///   An <see cref="object" /> representing the given <see cref="IDictionary" />, or <c>null</c> if the
        ///   object cannot be created.
        /// </returns>
        [NotNull]
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues is null)
                throw new ArgumentNullException(nameof(propertyValues));

            return new Half4((Half) propertyValues["X"], (Half) propertyValues["Y"], (Half) propertyValues["Z"], (Half) propertyValues["W"]);
        }

        /// <summary>
        ///   Determines whether changing a value on this object requires a call to <see cref="TypeConverter.CreateInstance(IDictionary)"/>
        ///   to create a new value, using the specified context.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>
        ///   <c>false</c> if changing a property on this object requires a call to <see cref="TypeConverter.CreateInstance(IDictionary)"/>
        ///   to create a new value; otherwise, <c>false</c>.
        /// </returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        ///   Creates an instance of the type that this <see cref="TypeConverter" /> is associated with, using the specified context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="value">An <see cref="object" /> that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type <see cref="Attribute" /> that is used as a filter.</param>
        /// <returns>
        ///   A <see cref="PropertyDescriptorCollection" /> with the properties that are exposed for this data type,
        ///   or a <c>null</c> reference if there are no properties.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return properties;
        }

        /// <summary>
        ///   Determines whether this object supports properties, using the specified context.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>
        ///   <c>true</c> if <see cref="GetProperties"/> should be called to find the properties of this object;
        ///   otherwise, <c>false</c>.
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
