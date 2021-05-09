// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Reflection;

namespace Stride.Core.Settings
{
    /// <summary>
    ///   Manages the loading of application settings with <see cref="IAppSettingsProvider"/>.
    /// </summary>
    public static class AppSettingsManager
    {
        private static AppSettings settings;
        private static IAppSettingsProvider provider;

        /// <summary>
        ///   Gets <see cref="AppSettings"/> instance for the application.
        /// </summary>
        /// <remarks>
        ///   Loaded with an <see cref="IAppSettingsProvider"/>.
        ///   If no provider implementation is found, returns an empty <see cref="AppSettings"/> instance.
        /// </remarks>
        public static AppSettings Settings
        {
            get
            {
                if (settings is not null)
                    return settings;

                ReloadSettings();
                return settings;
            }
        }

        /// <summary>
        ///   Gets or sets an <see cref="IAppSettingsProvider"/> for the application.
        /// </summary>
        /// <value>
        ///   The configured application settings provider; or <c>null</c> if no provider was configured nor found.
        /// </value>
        /// <remarks>
        ///   If the provider is not set, the manager will attempt to find an implementation among the
        ///   registered assemblies and cache it.
        /// </remarks>
        public static IAppSettingsProvider SettingsProvider
        {
            get
            {
                if (provider is not null)
                    return provider;

                foreach (var assembly in AssemblyRegistry.FindAll())
                {
                    var scanTypes = AssemblyRegistry.GetScanTypes(assembly);
                    if (scanTypes is not null &&
                        scanTypes.Types.TryGetValue(typeof(IAppSettingsProvider), out var providerTypes))
                    {
                        foreach (var type in providerTypes)
                            if (!type.IsAbstract &&
                                type.GetConstructor(Type.EmptyTypes) is not null)
                            {
                                provider = (IAppSettingsProvider) Activator.CreateInstance(type);
                                return provider;
                            }
                    }
                }

                return null;
            }

            set => provider = value;
        }

        /// <summary>
        ///   Clears the cached setting values and calls <see cref="IAppSettingsProvider.LoadAppSettings"/> on
        ///   the <see cref="SettingsProvider"/>.
        /// </summary>
        public static void ReloadSettings()
        {
            if (SettingsProvider is not null)
                settings = SettingsProvider.LoadAppSettings();
            else
                settings = new AppSettings();
        }
    }
}
