// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.ViewModels;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Core.Assets.Editor.View.ValueConverters
{
    public class IsFirstChild : OneWayValueConverter<IsFirstChild>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var node = (NodeViewModel)value;
            return node.Parent != null && node.Parent.Children.First(x => x.IsVisible) == node;
        }
    }
}
