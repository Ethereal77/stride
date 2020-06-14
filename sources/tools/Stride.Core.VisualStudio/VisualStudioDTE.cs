// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#if STRIDE_RUNTIME_NETFW
using EnvDTE;
#endif

using Process = System.Diagnostics.Process;

namespace Stride.Core.VisualStudio
{
    /// <summary>
    ///   Helper class to locate Visual Studio instances.
    /// </summary>
    class VisualStudioDTE
    {
#if STRIDE_RUNTIME_NETFW
        public static IEnumerable<Process> GetActiveInstances()
        {
            return GetActiveDTEs().Select(x => x.ProcessId).Select(Process.GetProcessById);
        }

        public static DTE GetDTEByProcess(int processId)
        {
            return GetActiveDTEs().FirstOrDefault(x => x.ProcessId == processId).DTE;
        }

        /// <summary>
        ///   Gets the instances of active <see cref="DTE"/>s.
        /// </summary>
        /// <returns>A collection of the active <see cref="DTE"/>s.</returns>
        internal static IEnumerable<Instance> GetActiveDTEs()
        {
            if (GetRunningObjectTable(0, out IRunningObjectTable rot) == 0)
            {
                rot.EnumRunning(out IEnumMoniker enumMoniker);

                var moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, IntPtr.Zero) == 0)
                {
                    CreateBindCtx(0, out IBindCtx bindCtx);
                    moniker[0].GetDisplayName(bindCtx, null, out string displayName);

                    // Check if it's Visual Studio
                    if (displayName.StartsWith("!VisualStudio"))
                    {
                        rot.GetObject(moniker[0], out object obj);

                        if (obj is DTE dte)
                        {
                            yield return new Instance
                            {
                                DTE = dte,
                                ProcessId = int.Parse(displayName.Split(':')[1])
                            };
                        }
                    }
                }
            }
        }

        public struct Instance
        {
            public DTE DTE;
            public int ProcessId;
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
#else
        public static IEnumerable<Process> GetActiveInstances()
        {
            return Array.Empty<Process>();
        }
#endif
    }
}
