// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;

using Stride.Core.Assets.Editor.Services;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Core.Assets.Editor.View.ValueConverters
{
    public class AssetViewModelToUrl : ValueConverterBase<AssetViewModelToUrl>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var asset = (AssetViewModel)value;
            return asset != null && asset.Id != AssetId.Empty ? asset.Url : ContentReferenceHelper.EmptyReference;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;
#pragma warning disable 618 // for AllAssets
            var asset = SessionViewModel.Instance.AllAssets.FirstOrDefault(x => x.Url == url);
#pragma warning restore 618
            return asset;
        }
    }
}
