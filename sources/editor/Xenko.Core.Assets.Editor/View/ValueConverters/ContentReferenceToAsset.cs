// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Core.Serialization;
using Xenko.Core.Serialization.Contents;
using Xenko.Core.Presentation.Quantum.ViewModels;
using Xenko.Core.Presentation.ValueConverters;

namespace Xenko.Core.Assets.Editor.View.ValueConverters
{
    public class ContentReferenceToAsset : OneWayValueConverter<ContentReferenceToAsset>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Empty reference or different values
            if (value == null || value == NodeViewModel.DifferentValues)
                return null;

            var contentReference = value as IReference ?? AttachedReferenceManager.GetAttachedReference(value);
            return contentReference != null ? SessionViewModel.Instance.GetAssetById(contentReference.Id) : null;
        }
    }
}
