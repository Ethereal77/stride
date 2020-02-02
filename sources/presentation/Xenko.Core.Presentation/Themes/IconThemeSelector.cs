// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows.Media;

namespace Xenko.Core.Presentation.Themes
{
    /// <summary>
    /// Contains a predefined set of <see cref="IconTheme"/>
    /// </summary>
    public static class IconThemeSelector
    {
        public enum KnownThemes
        {
            Light,
            Dark
        }

        public static IconTheme GetIconTheme(this KnownThemes theme)
        {
            switch (theme)
            {
                case KnownThemes.Dark:
                    return new IconTheme("Dark", Color.FromRgb(16, 16, 17));

                case KnownThemes.Light: 
                    return new IconTheme("Light", Color.FromRgb(245, 245, 245));

                default:
                    return default(IconTheme);
            }
        }
    }
}
