// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Mathematics;

namespace Stride.Core.Presentation.ValueConverters
{
    public class AngleSingleToDegrees : ValueConverterBase<AngleSingleToDegrees>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return targetType == typeof(double) ? ConverterHelper.ConvertToAngleSingle(value, culture).Degrees : ConverterHelper.TryConvertToAngleSingle(value, culture)?.Degrees;
        }

        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = targetType == typeof(AngleSingle) ? ConverterHelper.ConvertToDouble(value, culture) : ConverterHelper.TryConvertToDouble(value, culture);
            return doubleValue != null ? (object)new AngleSingle((float)doubleValue.Value, AngleType.Degree) : null;
        }
    }
}
