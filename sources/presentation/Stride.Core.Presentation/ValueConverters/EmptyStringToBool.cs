// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Presentation.Internal;

namespace Stride.Core.Presentation.ValueConverters
{
    /// <summary>
    /// This converter will convert a string value to a boolean value, returning <c>true</c> if the string is null or empty (or whitespace, see remarks), <c>false</c> otherwise.
    /// </summary>
    /// <remarks>
    /// If the boolean value <c>true</c> is passed as converter parameter, a whitespace string (<see cref="string.IsNullOrWhiteSpace(string)"/>)
    /// is also considered empty.
    /// </remarks>
    public class EmptyStringToBool : OneWayValueConverter<EmptyStringToBool>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = ConverterHelper.ConvertToString(value, culture);
            var result = parameter is bool && (bool)parameter
                ? string.IsNullOrWhiteSpace(stringValue)
                : string.IsNullOrEmpty(stringValue);
            return result.Box();
        }
    }
}
