// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Globalization;

using NuGet.Frameworks;

using Stride.Core.Annotations;
using Stride.Core.Presentation.ValueConverters;

namespace Stride.LauncherApp.ViewModels
{
    internal class FrameworkConverter : ValueConverterBase<FrameworkConverter>
    {
        public override object Convert(object value, [NotNull] Type targetType, object parameter, CultureInfo culture)
        {
            var frameworkFolder = (string) value;

            var framework = NuGetFramework.ParseFolder(frameworkFolder);
            if (framework.Framework == ".NETFramework")
                return $".NET {framework.Version.ToString(3)}";
            else if (framework.Framework == ".NETCoreApp")
                return $".NET Core {framework.Version.ToString(2)}";

            // Fallback
            return $"{framework.Framework} {framework.Version.ToString(3)}";
        }

        public override object ConvertBack(object value, [NotNull] Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
