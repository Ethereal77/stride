// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Copyright (c) 2007-2011 SlimDX Group
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

using Half = Stride.Core.Mathematics.Half;

namespace Stride.Core.TypeConverters
{
    /// <summary>
    ///   Provides a type converter to convert <see cref="Half"/> objects to and from various
    ///   other representations.
    /// </summary>
    public class HalfConverter : ExpandableObjectConverter
    {
        /// <summary>
        ///   Determines whether this converter can convert an object of the given type to the type of this converter,
        ///   using the specified context.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns>
        ///   <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        ///   Determines whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to.</param>
        /// <returns>
        ///   <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
        /// </returns>
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
        /// <param name="context">A <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">A <see cref="CultureInfo"/>. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (culture is null)
                culture = CultureInfo.CurrentCulture;

            if (!(value is string @string))
                return base.ConvertFrom(context, culture, value);

            @string = @string.Trim();
            char[] separator = new char[] { culture.TextInfo.ListSeparator[0] };
            string[] stringArray = @string.Split(separator);
            if (stringArray.Length != 1)
                throw new ArgumentException("Invalid half format.");

            float h = (float) TypeDescriptor.GetConverter(typeof(float)).ConvertFromString(context, culture, stringArray[0]);
            Half type = new Half(h);
            return type;
        }

        /// <summary>
        ///   Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">A <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">A <see cref="CultureInfo"/>. If <c>null</c> is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <param name="destinationType">A <see cref="Type"/> that represents the type you want to convert to.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType is null)
                throw new ArgumentNullException(nameof(destinationType));

            if (culture is null)
                culture = CultureInfo.CurrentCulture;

            if (destinationType == typeof(string) && value is Half half)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
                return string.Join(culture.TextInfo.ListSeparator + " ", new string[] { converter.ConvertToString(context, culture, (float) half) });
            }
            if (destinationType == typeof(InstanceDescriptor) && value is Half half1)
            {
                var info = typeof(Half).GetConstructor(new Type[] { typeof(float) });
                if (info != null)
                    return new InstanceDescriptor(info, new object[] { (float) half1 });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
