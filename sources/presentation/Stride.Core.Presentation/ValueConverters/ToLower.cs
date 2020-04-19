// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

namespace Xenko.Core.Presentation.ValueConverters
{
    /// <summary>
    /// Transforms string into lower case.
    /// </summary>
    public class ToLower : OneWayValueConverter<ToLower>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.ToLower(culture);
        }
    }
}
