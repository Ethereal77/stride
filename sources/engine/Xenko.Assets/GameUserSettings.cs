// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets;
using Xenko.Core.Settings;
using Xenko.Engine.Design;

namespace Xenko.Assets
{
    public static class GameUserSettings
    {
        public static class Effect
        {
            public static SettingsKey<EffectCompilationMode> EffectCompilation = new SettingsKey<EffectCompilationMode>("Package/Game/Effect/EffectCompilation", PackageUserSettings.SettingsContainer, EffectCompilationMode.LocalOrRemote)
            {
                DisplayName = "Effect Compiler"
            };
            public static SettingsKey<bool> RecordUsedEffects = new SettingsKey<bool>("Package/Game/Effect/RecordUsedEffects", PackageUserSettings.SettingsContainer, true)
            {
                DisplayName = "Record used effects"
            };
        }
    }
}
