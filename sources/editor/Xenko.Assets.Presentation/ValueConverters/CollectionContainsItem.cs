// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Globalization;
using System.Linq;

using Xenko.Core.Presentation.ValueConverters;

namespace Xenko.Assets.Presentation.ValueConverters
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
