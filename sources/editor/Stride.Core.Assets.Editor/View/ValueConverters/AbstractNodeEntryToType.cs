// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

using Stride.Core.Assets.Editor.Quantum.NodePresenters.Commands;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Core.Assets.Editor.View.ValueConverters
{
    public class AbstractNodeEntryToType : OneWayValueConverter<AbstractNodeEntryToType>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var entry = value as AbstractNodeType;
            return entry?.Type;
        }
    }
}
