// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Presentation.ValueConverters;
using Stride.Assets.Presentation.AssetEditors.UIEditor.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.UIEditor.Views
{
    internal class UIElementFactoryTypeConverter : OneWayValueConverter<UIElementFactoryTypeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is UIElementFromSystemLibrary ? "Standard library" : "User libraries";
        }
    }
}
