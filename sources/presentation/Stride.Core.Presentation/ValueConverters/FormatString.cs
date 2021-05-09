// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows;

using Stride.Core.Annotations;

namespace Stride.Core.Presentation.ValueConverters
{
    public class FormatString : OneWayValueConverter<FormatString>
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue)
                return value;

            var format = parameter as string;
            return string.Format(format ?? "{0}", value);
        }
    }
}
