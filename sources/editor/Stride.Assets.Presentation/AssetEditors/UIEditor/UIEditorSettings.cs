// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Settings;
using Stride.Core.Translation;

namespace Stride.Assets.Presentation.AssetEditors.UIEditor
{
    internal static class UIEditorSettings
    {
        // Categories
        public static readonly string UIEditor = Tr._p("Settings", "UI editor");

        static UIEditorSettings()
        {
            // Note: assignments cannot be moved to initializer, because category names need to be assigned first.
            AskBeforeDeletingUIElements = new SettingsKey<bool>("UIEditor/AskBeforeDeletingUIElements", Stride.Core.Assets.Editor.Settings.EditorSettings.SettingsContainer, true)
            {
                DisplayName = $"{UIEditor}/{Tr._p("Settings", "Ask before deleting UI elements")}"
            };
        }

        public static SettingsKey<bool> AskBeforeDeletingUIElements { get; }

        public static void Save()
        {
            Stride.Core.Assets.Editor.Settings.EditorSettings.Save();
        }
    }
}
