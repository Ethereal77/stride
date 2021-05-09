// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Stride.Core;

namespace Stride.Core.Presentation.ValueConverters
{
    public class EnumToDisplayName : OneWayValueConverter<EnumToDisplayName>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "(None)";

            var stringValue = value.ToString();
            var type = value.GetType();
            var memberInfo = type.GetMember(stringValue).FirstOrDefault();
            if (memberInfo == null)
                return stringValue;

            var attribute = memberInfo.GetCustomAttribute(typeof(DisplayAttribute), false) as DisplayAttribute;
            if (attribute == null)
                return stringValue;

            return attribute.Name;
        }
    }
}
