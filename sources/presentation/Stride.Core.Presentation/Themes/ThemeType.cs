// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Themes
{
    public enum ThemeType
    {
        // Dark themes
        [Display("Expression Dark (Default)")]
        ExpressionDark,
        [Display("Dark Steel")]
        DarkSteel,

        // Light themes
        [Display("Light Steel Blue (Experimental)")]
        LightSteelBlue,
    }

    public static class ThemeTypeExtensions
    {
        public static IconThemeSelector.ThemeBase GetThemeBase(this ThemeType themeType)
        {
            switch (themeType)
            {
                case ThemeType.ExpressionDark:
                case ThemeType.DarkSteel:
                default:
                    return IconThemeSelector.ThemeBase.Dark;

                case ThemeType.LightSteelBlue:
                    return IconThemeSelector.ThemeBase.Light;
            }
        }
    }
}
