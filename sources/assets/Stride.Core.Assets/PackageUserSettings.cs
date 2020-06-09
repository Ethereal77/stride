// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Extensions;
using Stride.Core.Settings;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   Represents the user settings related to a <see cref="Package"/>, usually stored in a <c>.user</c> file along the package file.
    /// </summary>
    public class PackageUserSettings
    {
        private const string SettingsExtension = ".user";

        public static SettingsContainer SettingsContainer = new SettingsContainer();

        private readonly Package package;

        public SettingsProfile Profile { get; }


        internal PackageUserSettings(Package package)
        {
            if (package is null)
                throw new ArgumentNullException(nameof(package));

            this.package = package;

            if (package.FullPath is null)
            {
                Profile = SettingsContainer.CreateSettingsProfile(setAsCurrent: false);
            }
            else
            {
                var path = package.FullPath + SettingsExtension;
                try
                {
                    Profile = SettingsContainer.LoadSettingsProfile(path, setAsCurrent: false);
                }
                catch (Exception e)
                {
                    e.Ignore();
                }

                if (Profile is null)
                    Profile = SettingsContainer.CreateSettingsProfile(setAsCurrent: false);
            }
        }

        public bool Save()
        {
            if (package.FullPath is null)
                return false;

            var path = package.FullPath + SettingsExtension;
            return SettingsContainer.SaveSettingsProfile(Profile, path);
        }

        public T GetValue<T>(SettingsKey<T> key) => key.GetValue(Profile, searchInParentProfile: true);

        public void SetValue<T>(SettingsKey<T> key, T value) => key.SetValue(value, Profile);
    }
}
