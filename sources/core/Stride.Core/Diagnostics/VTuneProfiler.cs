// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Stride.Core.Annotations;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Defines methods to access the APIs of Intel VTune Profiler, a performance analysis tool for serial and multithreaded applications.
    /// </summary>
    /// <seealso href="https://software.intel.com/content/www/us/en/develop/documentation/vtune-help/top.html">Intel VTune Profiler</seealso>
    public static class VTuneProfiler
    {
        private const string VTune2015DllName = "ittnotify_collector.dll";

        private static readonly Dictionary<string, StringHandle> stringHandles = new();

        /// <summary>
        ///   Resumes the profiler.
        /// </summary>
        public static void Resume()
        {
            try
            {
                __itt_resume();
            }
            catch (DllNotFoundException) { }
        }

        /// <summary>
        ///   Suspends the profiler.
        /// </summary>
        public static void Pause()
        {
            try
            {
                __itt_pause();
            }
            catch (DllNotFoundException) { }
        }

        /// <summary>
        ///   A value indicating whether the Intel VTune profiling API is available and has been loaded succesfully.
        /// </summary>
        public static readonly bool IsAvailable = NativeLibrary.TryLoad(VTune2015DllName, out _);

        /// <summary>
        ///   Creates a new event with the specified name.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <returns>The event.</returns>
        public static Event CreateEvent([NotNull] string eventName)
        {
            if (eventName is null)
                throw new ArgumentNullException(nameof(eventName));

            return IsAvailable ? __itt_event_createW(eventName, eventName.Length) : new Event();
        }

        /// <summary>
        ///   Creates a new domain with the specified name.
        /// </summary>
        /// <param name="domaiName">Name of the domain.</param>
        /// <returns>The domain.</returns>
        public static Domain CreateDomain([NotNull] string domaiName)
        {
            if (domaiName is null)
                throw new ArgumentNullException(nameof(domaiName));

            return IsAvailable ? __itt_domain_createW(domaiName) : new Domain();
        }


        /// <summary>
        ///   A profiling event that can be use to observe when demarcated events occur in the application, or to identify how
        ///   long it takes to execute demarcated regions of code.
        /// </summary>
        /// <remarks>
        ///   The Event API is a per-thread function that works in resumed state. This function does not work in paused state.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct Event
        {
            private readonly int id;

            /// <summary>
            ///   Marks the start of this event.
            /// </summary>
            public void Start()
            {
                if (id == 0)
                    return;

                __itt_event_start(this);
            }

            /// <summary>
            ///   Marks the end of this event.
            /// </summary>
            public void End()
            {
                if (id == 0)
                    return;

                __itt_event_end(this);
            }
        }


        /// <summary>
        ///   A profiling domain that can be used to tag trace data for different modules or libraries in a program.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Domain
        {
            private readonly IntPtr pointer;

            /// <summary>
            ///   Marks the beginning of a frame.
            /// </summary>
            /// <remarks>
            ///   The Frame API is a per-process function that works in resumed state. This function does not work in paused state.
            /// </remarks>
            public void BeginFrame()
            {
                if (pointer == IntPtr.Zero)
                    return;

                __itt_frame_begin_v3(this, IntPtr.Zero);
            }

            /// <summary>
            ///   Marks the beginning of a task for the current thread.
            /// </summary>
            /// <param name="taskName">The name of the task.</param>
            /// <remarks>
            ///   A task is a logical unit of work performed by a particular thread. Tasks can nest; thus, tasks typically correspond
            ///   to functions, scopes, or a case block in a switch statement. You can use the Task API to assign tasks to threads.
            ///   <para/>
            ///   Tasks are a per-thread function that works in resumed state. This function does not work in paused state.
            ///   <para/>
            ///   The Task API does not enable a thread to suspend the current task and switch to a different task (<em>task switching</em>),
            ///   or move a task to a different thread (<em>task stealing</em>).
            /// </remarks>
            public void BeginTask(string taskName)
            {
                if (pointer == IntPtr.Zero)
                    return;

                __itt_task_begin(this, new IttId(), new IttId(), GetStringHandle(taskName));
            }

            /// <summary>
            ///   Marks the end of the current task for the current thread.
            /// </summary>
            /// <remarks>
            ///   A task is a logical unit of work performed by a particular thread. Tasks can nest; thus, tasks typically correspond
            ///   to functions, scopes, or a case block in a switch statement. You can use the Task API to assign tasks to threads.
            ///   <para/>
            ///   Tasks are a per-thread function that works in resumed state. This function does not work in paused state.
            ///   <para/>
            ///   The Task API does not enable a thread to suspend the current task and switch to a different task (<em>task switching</em>),
            ///   or move a task to a different thread (<em>task stealing</em>).
            /// </remarks>
            public void EndTask()
            {
                if (pointer == IntPtr.Zero)
                    return;

                __itt_task_end(this);
            }

            /// <summary>
            ///   Marks the end of a frame.
            /// </summary>
            /// <remarks>
            ///   The Frame API is a per-process function that works in resumed state. This function does not work in paused state.
            /// </remarks>
            public void EndFrame()
            {
                if (pointer == IntPtr.Zero)
                    return;

                __itt_frame_end_v3(this, IntPtr.Zero);
            }
        }


        //
        // Gets a string handle for a string if it was already created, or creates a new one.
        //
        private static StringHandle GetStringHandle([NotNull] string text)
        {
            lock (stringHandles)
            {
                if (!stringHandles.TryGetValue(text, out StringHandle result))
                {
                    result = __itt_string_handle_createW(text);
                    stringHandles.Add(text, result);
                }
                return result;
            }
        }

        //
        // A handle to a text string.
        //
        [StructLayout(LayoutKind.Sequential)]
        private struct StringHandle
        {
            private readonly IntPtr ptr;
        }


        //
        // An internal Id.
        //
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        private struct IttId
        {
            private readonly long d1;
            private readonly long d2;
            private readonly long d3;
        }

#pragma warning disable SA1300 // Element must begin with upper-case letter
        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void __itt_resume();

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void __itt_pause();

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)] // not working
        private static extern void __itt_frame_begin_v3(Domain domain, IntPtr id);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)] // not working
        private static extern void __itt_frame_end_v3(Domain domain, IntPtr id);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Domain __itt_domain_createW([MarshalAs(UnmanagedType.LPWStr)] string domainName);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Event __itt_event_createW([MarshalAs(UnmanagedType.LPWStr)] string eventName, int eventNameLength);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int __itt_event_start(Event eventHandler);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int __itt_event_end(Event eventHandler);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern StringHandle __itt_string_handle_createW([MarshalAs(UnmanagedType.LPWStr)] string text);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void __itt_task_begin(Domain domain, IttId taskid, IttId parentid, StringHandle name);

        [DllImport(VTune2015DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void __itt_task_end(Domain domain);
#pragma warning restore SA1300 // Element must begin with upper-case letter
    }
}
