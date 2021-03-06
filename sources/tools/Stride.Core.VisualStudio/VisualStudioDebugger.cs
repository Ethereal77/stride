// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

#if STRIDE_RUNTIME_NETFW
using EnvDTE;
#endif

namespace Stride.Core.VisualStudio
{
    /// <summary>
    ///   Helper class to attach Visual Studio instances to a process for debugging.
    /// </summary>
    internal class VisualStudioDebugger : IDisposable
    {
#if STRIDE_RUNTIME_NETFW
        private readonly STAContext context;
        private readonly DTE dte;
#endif

        public int ProcessId { get; private set; }

#if STRIDE_RUNTIME_NETFW
        private VisualStudioDebugger(STAContext context, DTE dte, int processId)
        {
            this.context = context;
            this.dte = dte;
            this.ProcessId = processId;
        }

        public static VisualStudioDebugger GetByProcess(int processId)
        {
            var context = new STAContext();

            var instance = GetFirstOrDefaultDTE(context, x => x.ProcessId == processId);

            if (instance.DTE is null)
            {
                context.Dispose();
                return null;
            }

            return new VisualStudioDebugger(context, instance.DTE, instance.ProcessId);
        }

        public static VisualStudioDebugger GetAttached()
        {
            if (!System.Diagnostics.Debugger.IsAttached)
                return null;

            var context = new STAContext();

            var instance = GetFirstOrDefaultDTE(context, x =>
            {
                // Try multiple times, as DTE might report it is busy
                var debugger = x.DTE.Debugger;
                if (debugger.DebuggedProcesses == null)
                    return false;

                return debugger.DebuggedProcesses.OfType<EnvDTE.Process>().Any(debuggedProcess => debuggedProcess.ProcessID == System.Diagnostics.Process.GetCurrentProcess().Id);
            });

            if (instance.DTE is null)
            {
                context.Dispose();
                return null;
            }

            return new VisualStudioDebugger(context, instance.DTE, instance.ProcessId);
        }

        // Make this DTE attach to the newly created process
        public void AttachToProcess(int processId)
        {
            context.Execute(() =>
            {
                MessageFilter.Register();
                var processes = dte.Debugger.LocalProcesses.OfType<EnvDTE.Process>();
                var process = processes.FirstOrDefault(x => x.ProcessID == processId);
                process?.Attach();
                MessageFilter.Revoke();
            });
        }

        // Make this DTE detach from the process
        public void DetachFromProcess(int processId)
        {
            context.Execute(() =>
            {
                MessageFilter.Register();
                var processes = dte.Debugger.LocalProcesses.OfType<EnvDTE.Process>();
                var process = processes.FirstOrDefault(x => x.ProcessID == processId);
                process?.Detach();
                MessageFilter.Revoke();
            });
        }

        public void Attach()
        {
            AttachToProcess(System.Diagnostics.Process.GetCurrentProcess().Id);
        }

        public void Detach()
        {
            DetachFromProcess(System.Diagnostics.Process.GetCurrentProcess().Id);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private static VisualStudioDTE.Instance GetFirstOrDefaultDTE(STAContext context, Func<VisualStudioDTE.Instance, bool> predicate)
        {
            return context.Execute(() =>
            {
                // Locate all Visual Studio DTE
                var dtes = VisualStudioDTE.GetActiveDTEs().ToArray();

                // Find DTE
                MessageFilter.Register();
                var result = dtes.FirstOrDefault(predicate);
                MessageFilter.Revoke();

                return result;
            });
        }
#else
        public static VisualStudioDebugger GetByProcess(int processId)
        {
            return null;
        }

        public static VisualStudioDebugger GetAttached()
        {
            return null;
        }

        public void Attach()
        {
            throw new PlatformNotSupportedException("EnvDTE is not supported with this runtime");
        }

        public void Detach()
        {
            throw new PlatformNotSupportedException("EnvDTE is not supported with this runtime");
        }

        public void AttachToProcess(int processId)
        {
            throw new PlatformNotSupportedException("EnvDTE is not supported with this runtime");
        }

        public void Dispose()
        {
            throw new PlatformNotSupportedException("EnvDTE is not supported with this runtime");
        }
#endif
    }
}
