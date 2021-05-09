// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System.IO;
using System.Reflection;

using Stride.Core.Yaml;

namespace Stride.Core.Settings
{
    /// <summary>
    ///   Default implementation of <see cref="IAppSettingsProvider"/> which uses a YAML deserializer to read the settings file.
    /// </summary>
    internal class AppSettingsProvider : IAppSettingsProvider
    {
        private const string SettingsExtension = ".appsettings";

        /// <inheritdoc/>
        public AppSettings LoadAppSettings()
        {
            var execFilePath = Assembly.GetEntryAssembly()?.Location;
            if (execFilePath is null)
                return new AppSettings();

            var settingsFilePath = Path.ChangeExtension(execFilePath, SettingsExtension);
            try
            {
                return YamlSerializer.Load<AppSettings>(settingsFilePath);
            }
            catch (FileNotFoundException)
            {
                return new AppSettings();
            }
        }
    }
}
