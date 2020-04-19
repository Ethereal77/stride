// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Assets.Editor.View.ValueConverters;
using Xenko.Engine;
using Xenko.Core.Presentation.ValueConverters;

namespace Xenko.Assets.Presentation.ValueConverters
{
    public class EntityComponentToResource : OneWayValueConverter<EntityComponentToResource>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var components = (IEnumerable<EntityComponent>)value;
            var componentTypes = components.Select(x => x.GetType());
            var compToUse = XenkoDefaultAssetsPlugin.GetHighestOrderComponent(componentTypes);
            return TypeToResource.FetchResourceFromType(compToUse, true);
        }
    }
}
