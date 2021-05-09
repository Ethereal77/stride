// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

using Stride.Engine;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Assets.Presentation.ValueConverters
{
    public class EntityComponentToTransformLinkInfo : OneWayValueConverter<EntityComponentToTransformLinkInfo>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var components = (IEnumerable<EntityComponent>)value;
            var modelNodeLinkComponent = components.OfType<ModelNodeLinkComponent>().FirstOrDefault();
            if (string.IsNullOrEmpty(modelNodeLinkComponent?.NodeName))
                return DependencyProperty.UnsetValue;

            if (modelNodeLinkComponent.Target != null && !string.IsNullOrEmpty(modelNodeLinkComponent.NodeName))
            {
                var entity = modelNodeLinkComponent.Target.Entity;
                return $"{entity.Name}.{modelNodeLinkComponent.NodeName}";
            }
            return modelNodeLinkComponent.NodeName;
        }
    }
}
