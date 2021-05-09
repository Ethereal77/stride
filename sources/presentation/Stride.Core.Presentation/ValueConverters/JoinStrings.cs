// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.ValueConverters
{
    /// <summary>
    /// This value converter will join an enumerable of strings with the separator given as parameter (or using a single space character as separator
    /// if the parameter is null).
    /// </summary>
    public class JoinStrings : ValueConverterBase<JoinStrings>
    {
        /// <inheritdoc/>
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strings = (IEnumerable<string>)value;
            var separator = (string)parameter;
            return string.Join(separator ?? " ", strings);
        }

        /// <inheritdoc/>
        [NotNull]
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            var separator = (string)parameter;
            return str.Split(new[] { separator ?? " "}, StringSplitOptions.None);
        }
    }
}
