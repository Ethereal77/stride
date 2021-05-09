// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Core.Settings;
using Stride.Engine.Design;

namespace Stride.Assets
{
    public static class GameUserSettings
    {
        public static class Effect
        {
            public static SettingsKey<EffectCompilationMode> EffectCompilation = new SettingsKey<EffectCompilationMode>("Package/Game/Effect/EffectCompilation", PackageUserSettings.SettingsContainer, EffectCompilationMode.Local)
            {
                DisplayName = "Effect Compiler"
            };
            public static SettingsKey<bool> RecordUsedEffects = new SettingsKey<bool>("Package/Game/Effect/RecordUsedEffects", PackageUserSettings.SettingsContainer, false)
            {
                DisplayName = "Record used effects"
            };
        }
    }
}
