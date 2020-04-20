// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;

using Stride.Core.Presentation.ValueConverters;

namespace Stride.LauncherApp.Views
{
    public class ProgressToIndeterminatedConverter : OneWayValueConverter<ProgressToIndeterminatedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var progress = (int)System.Convert.ChangeType(value, typeof(int));
            return progress <= 0;
        }
    }
}
