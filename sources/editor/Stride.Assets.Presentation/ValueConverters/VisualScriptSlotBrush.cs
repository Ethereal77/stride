// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Xenko.Core.Presentation.ValueConverters;
using Xenko.Assets.Presentation.AssetEditors.VisualScriptEditor;

namespace Xenko.Assets.Presentation.ValueConverters
{
    public abstract class VisualScriptSlotBrush<T> : OneWayValueConverter<T> where T : class, IValueConverter, new()
    {
        protected abstract string ResourceSuffix { get; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var slot = (VisualScriptSlotViewModel)value;
            var kind = slot.Kind;

            var frameworkElement = parameter as FrameworkElement;
            return frameworkElement?.TryFindResource(kind + ResourceSuffix);
        }
    }

    public class VisualScriptSlotBrushConnectorFill : VisualScriptSlotBrush<VisualScriptSlotBrushConnectorFill>
    {
        protected override string ResourceSuffix => "ConnectorFill";
    }

    public class VisualScriptSlotBrushMouseOverConnectorFill : VisualScriptSlotBrush<VisualScriptSlotBrushMouseOverConnectorFill>
    {
        protected override string ResourceSuffix => "MouseOverConnectorFill";
    }

    public class VisualScriptSlotBrushConnectorStroke : VisualScriptSlotBrush<VisualScriptSlotBrushConnectorStroke>
    {
        protected override string ResourceSuffix => "ConnectorStroke";
    }
}
