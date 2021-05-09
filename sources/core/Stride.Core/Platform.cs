// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Stride.Core
{
    /// <summary>
    ///   Defines platform specific queries and functions.
    /// </summary>
    public static class Platform
    {
        /// <summary>
        ///   The current running <see cref="PlatformType"/>.
        /// </summary>
        public static readonly PlatformType Type = PlatformType.Windows;

        /// <summary>
        ///   Gets a value indicating whether the running platform is a desktop version of Windows.
        /// </summary>
        /// <value><c>true</c> if this instance is desktop Windows; otherwise, <c>false</c>.</value>
        public static readonly bool IsWindowsDesktop = Type == PlatformType.Windows;

        /// <summary>
        ///   Gets a value indicating whether the running assembly is a debug assembly.
        /// </summary>
        public static readonly bool IsRunningDebugAssembly = GetIsRunningDebugAssembly();


        /// <summary>
        ///   Checks if running assembly has the DebuggableAttribute set with the `DisableOptimizations` mode enabled.
        ///   This function is called only once.
        /// </summary>
        private static bool GetIsRunningDebugAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var debuggableAttribute = entryAssembly.GetCustomAttributes<DebuggableAttribute>().FirstOrDefault();
                if (debuggableAttribute != null)
                {
                    return debuggableAttribute.DebuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.DisableOptimizations);
                }
            }
            return false;
        }
    }
}
