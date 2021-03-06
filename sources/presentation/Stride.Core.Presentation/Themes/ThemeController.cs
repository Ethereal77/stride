// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;

namespace Stride.Core.Presentation.Themes
{
    /// <summary>
    /// This class contains properties to control theming of icons, etc.
    /// </summary>
    public static class ThemeController
    {
        /// <summary>
        /// The main purpose of this property is for Luminosity Check feature of
        /// <see cref="ImageThemingUtilities.TransformDrawing(Media.Drawing, IconTheme, bool)"/>.
        /// </summary>
        public static readonly DependencyProperty IsDarkProperty =
            DependencyProperty.RegisterAttached("IsDark", typeof(bool), typeof(ThemeController), new PropertyMetadata(false));

        public static bool GetIsDark(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDarkProperty);
        }

        public static void SetIsDark(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDarkProperty, value);
        }

        public static ThemeType CurrentTheme { get; set; }
    }
}
