// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Presentation.ValueConverters;

namespace Xenko.Core.Assets.Editor.View.ValueConverters
{
    public class EnumToResource : OneWayValueConverter<EnumToResource>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Value can be null when the control is removed from the visual tree and the related property is unbound.
            return value == null ? null : SessionViewModel.Instance.ServiceProvider.Get<IAssetsPluginService>().GetImageForEnum(SessionViewModel.Instance, value);
        }
    }
}
