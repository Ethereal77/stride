// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.Core.Assets.Editor.View.ValueConverters
{
    public class AssetToExpandedAtInitialization : OneWayValueConverter<AssetToExpandedAtInitialization>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int assetLevel = 0;
            if (parameter is int)
            {
                assetLevel = (int)parameter;
            }

            var directory = value as DirectoryBaseViewModel;
            if (directory != null)
            {
                return directory.Level < assetLevel;
            }

            var project = value as PackageViewModel;
            if (project != null)
            {
                // Projects are always expanded by default
                return true;
            }

            var packageCategory = value as PackageCategoryViewModel;
            if (packageCategory != null)
            {
                // Only the local package category should be expanded by default
                return packageCategory == packageCategory.Session.PackageCategories[SessionViewModel.LocalPackageCategoryName];
            }

            return false;
        }
    }
}
