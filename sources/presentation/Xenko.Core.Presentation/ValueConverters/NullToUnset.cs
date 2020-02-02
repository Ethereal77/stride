// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.Windows;

namespace Xenko.Core.Presentation.ValueConverters
{
    /// <summary>
    /// This converter will convert a null value to <see cref="DependencyProperty.UnsetValue"/>. If the given object is not null, it will be returned as it is.
    /// </summary>
    public class NullToUnset : OneWayValueConverter<NullToUnset>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DependencyProperty.UnsetValue;
        }
    }
}
