// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using System.Windows;

using Stride.Core.Annotations;
using Stride.Core.Presentation.Internal;

namespace Stride.Core.Presentation.ValueConverters
{
    public class AllEqualMultiConverter : OneWayMultiValueConverter<AllEqualMultiConverter>
    {
        [NotNull]
        public override object Convert([NotNull] object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                throw new InvalidOperationException("This multi converter must be invoked with at least two elements");

            var fallbackValue = parameter is bool && (bool)parameter;
            var first = values[0];
            var result = values.All(x => x == DependencyProperty.UnsetValue ? fallbackValue : Equals(x, first));
            return result.Box();
        }
    }
}
