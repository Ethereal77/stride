// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using Stride.Core.Annotations;

namespace Stride.Core.Presentation.ValueConverters
{
    public class DateTimeToString : ValueConverterBase<DateTimeToString>
    {
        public override object Convert(object value, [NotNull] Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            DateTime dateTime = (DateTime)value;
            return dateTime.ToString(culture);
        }

        public override object ConvertBack(object value, [NotNull] Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string stringValue = value.ToString();
            return DateTime.Parse(stringValue, culture);
        }
    }
}
