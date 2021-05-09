// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows.Media;

namespace Stride.Core.Presentation.Themes
{
    /// <summary>
    /// Contains a predefined set of <see cref="IconTheme"/>
    /// </summary>
    public static class IconThemeSelector
    {
        public enum ThemeBase
        {
            Light,
            Dark
        }

        public static IconTheme GetIconTheme(this ThemeBase theme)
        {
            switch (theme)
            {
                case ThemeBase.Dark:
                    return new IconTheme("Dark", Color.FromRgb(16, 16, 17));

                case ThemeBase.Light:
                    return new IconTheme("Light", Color.FromRgb(245, 245, 245));

                default:
                    return default(IconTheme);
            }
        }
    }
}
