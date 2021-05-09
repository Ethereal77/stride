// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Reflection;

namespace Stride.Core.Settings
{
    /// <summary>
    ///   Defines the interface of custom loader for the application settings.
    /// </summary>
    /// <remarks>
    ///   The implementer of this class is required to have a parameterless constructor.
    ///   <para/>
    ///   We don't want a dependency on complex parsing libraries in Stride.Core,
    ///   so the reading of the <see cref="AppSettings"/> file is left to the implementation of this interface.
    /// </remarks>
    [AssemblyScan]
    public interface IAppSettingsProvider
    {
        /// <summary>
        ///   Loads <see cref="AppSettings"/> for the application.
        /// </summary>
        AppSettings LoadAppSettings();
    }
}
