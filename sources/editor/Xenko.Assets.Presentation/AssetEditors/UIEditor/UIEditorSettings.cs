// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Settings;
using Xenko.Core.Translation;

namespace Xenko.Assets.Presentation.AssetEditors.UIEditor
{
    internal static class UIEditorSettings
    {
        // Categories
        public static readonly string UIEditor = Tr._p("Settings", "UI editor");

        static UIEditorSettings()
        {
            // Note: assignments cannot be moved to initializer, because category names need to be assigned first.
            AskBeforeDeletingUIElements = new SettingsKey<bool>("UIEditor/AskBeforeDeletingUIElements", Xenko.Core.Assets.Editor.Settings.EditorSettings.SettingsContainer, true)
            {
                DisplayName = $"{UIEditor}/{Tr._p("Settings", "Ask before deleting UI elements")}"
            };
        }

        public static SettingsKey<bool> AskBeforeDeletingUIElements { get; }

        public static void Save()
        {
            Xenko.Core.Assets.Editor.Settings.EditorSettings.Save();
        }
    }
}
