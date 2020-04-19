// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

using Xenko.Core.Presentation.Internal;

namespace Xenko.Core.Presentation.ValueConverters
{
    /// <summary>
    /// This converter will invert a boolean value. <see cref="ConvertBack"/> is supported and does basically the same operation as <see cref="Convert"/>.
    /// </summary>
    public class InvertBool : ValueConverterBase<InvertBool>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = !ConverterHelper.ConvertToBoolean(value, culture);
            return result.Box();
        }

        /// <inheritdoc/>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
