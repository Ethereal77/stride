// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Assets.Editor.Settings.ViewModels;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Core.Assets.Editor.View.ValueConverters
{
    public class SettingsCategoryToExpandedAtInitialization : OneWayValueConverter<SettingsCategoryToExpandedAtInitialization>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var category = (SettingsCategoryViewModel)value;
            return category.Parent == null;
        }
    }
}
