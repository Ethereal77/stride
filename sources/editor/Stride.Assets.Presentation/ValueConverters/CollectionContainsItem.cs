// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Globalization;
using System.Linq;

using Stride.Core.Presentation.ValueConverters;

namespace Stride.Assets.Presentation.ValueConverters
{
    public class CollectionContainsItem: OneWayMultiValueConverter<CollectionContainsItem>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0];
            var collection = values[1] as IEnumerable;
            return collection != null && collection.Cast<object>().Any(x => Equals(x, item));
        }
    }
}
