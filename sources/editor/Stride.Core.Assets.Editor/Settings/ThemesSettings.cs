// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.Settings;
using Stride.Core.Settings;
using Stride.Core.Translation;

namespace Stride.Core.Presentation.Themes
{
    public static class ThemesSettings
    {
        // Categories
        public static readonly string Themes = Tr._p("Settings", "Themes");

        static ThemesSettings()
        {
            ThemeName = new SettingsKey<ThemeType>("Themes/ThemeName", EditorSettings.SettingsContainer, ThemeType.ExpressionDark)
            {
                DisplayName = $"{Themes}/{Tr._p("Settings", "Theme Name")}"
            };
        }

        public static SettingsKey<ThemeType> ThemeName { get; }

        public static void Initialize()
        {
            ThemeController.CurrentTheme = ThemeName.GetValue();
        }
    }
}
