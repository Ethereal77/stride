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
    /// A converter that resolve the specified value from the resources from the current application
    /// </summary>
    public class StaticResourceConverter : OneWayValueConverter<StaticResourceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Application.Current.TryFindResource(value) ?? DependencyProperty.UnsetValue;
        }
    }
}
