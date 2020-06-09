// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.LauncherApp
{
    /// <summary>
    ///   Defines error codes returned by the Launcher process.
    /// </summary>
    public enum LauncherErrorCode
    {
        Success = 0,

        // Non-error values (positive)
        ServerAlreadyRunning = 1,

        // RunServer errors: -1 to -100
        ErrorWhileRunningServer = -1,           // We don't have a more accurate error at the moment
        ErrorWhileInitializingServer = -2,

        // UpdateTargets errors: -101 to -200
        ErrorUpdatingTargetFiles = -101,        // We don't have a more accurate error at the moment

        // Uninstall errors: -201 to -300
        UninstallCancelled = -201,
        ErrorWhileUninstalling = -202,          // We don't have a more accurate error at the moment

        UnknownError = -10000
    }
}
