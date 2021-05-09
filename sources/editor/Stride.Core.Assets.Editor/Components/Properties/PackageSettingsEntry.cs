// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Settings;

namespace Stride.Core.Assets.Editor.Components.Properties
{
    public struct PackageSettingsEntry
    {
        public SettingsKey SettingsKey;
        public TargetPackage TargetPackage;

        public PackageSettingsEntry(SettingsKey key, TargetPackage targetPackage = TargetPackage.All)
        {
            SettingsKey = key;
            TargetPackage = targetPackage;
        }
    }
}
