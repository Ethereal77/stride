// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;

namespace Xenko.Core.Presentation.Themes
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
    }
}
