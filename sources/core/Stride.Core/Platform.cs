// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Xenko.Core
{
    /// <summary>
    /// Platform specific queries and functions.
    /// </summary>
    public static class Platform
    {
        /// <summary>
        /// The current running <see cref="PlatformType"/>.
        /// </summary>
        public static readonly PlatformType Type = PlatformType.Windows;

        /// <summary>
        /// Gets a value indicating whether the running assembly is a debug assembly.
        /// </summary>
        public static readonly bool IsRunningDebugAssembly = GetIsRunningDebugAssembly();

        /// <summary>
        /// Check if running assembly has the DebuggableAttribute set with the `DisableOptimizations` mode enabled.
        /// This function is called only once.
        /// </summary>
        private static bool GetIsRunningDebugAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var debuggableAttribute = entryAssembly.GetCustomAttributes<DebuggableAttribute>().FirstOrDefault();
                if (debuggableAttribute != null)
                {
                    return (debuggableAttribute.DebuggingFlags & DebuggableAttribute.DebuggingModes.DisableOptimizations) != 0;
                }
            }
            return false;
        }
    }
}
