// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;

namespace Stride.Core.Presentation.ValueConverters
{
    /// <summary>
    /// This converter will convert a specific value to a <see cref="DependencyProperty.UnsetValue"/>.
    /// </summary>
    public class ValueToUnset : OneWayValueConverter<ValueToUnset>
    {
        /// <inheritdoc/>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter) ? DependencyProperty.UnsetValue : value;
        }
    }
}
