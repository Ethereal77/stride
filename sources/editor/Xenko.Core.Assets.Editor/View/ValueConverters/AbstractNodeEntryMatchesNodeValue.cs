// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using Xenko.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Xenko.Core.Presentation.ValueConverters;

namespace Xenko.Core.Assets.Editor.View.ValueConverters
{
    public abstract class AbstractNodeEntryMatchesNodeValue<T> : OneWayMultiValueConverter<T> where T : class, IMultiValueConverter, new()
    {
        protected abstract object ReturnValue { get; }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                throw new InvalidOperationException("This multi converter must be invoked with two elements");

            var entry = values[0] as AbstractNodeEntry;
            var value = values[1];

            return entry != null && entry.IsMatchingValue(value) ? ReturnValue : DependencyProperty.UnsetValue;
        }
    }

    public class AbstractNodeEntryToFontWeight : AbstractNodeEntryMatchesNodeValue<AbstractNodeEntryToFontWeight>
    {
        public static readonly FontWeight FontWeight = FontWeights.ExtraBold;

        protected override object ReturnValue { get { return FontWeight; } }
    }

    public class AbstractNodeEntryToBrush : AbstractNodeEntryMatchesNodeValue<AbstractNodeEntryToBrush>
    {
        public static readonly Brush Brush = new SolidColorBrush(Colors.SkyBlue);

        protected override object ReturnValue { get { return Brush; } }
    }
}
