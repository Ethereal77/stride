// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Core.MicroThreading;
using Stride.Core.Serialization.Contents;
using Stride.Games;

namespace Stride.Engine.Processors
{
    /// <summary>
    ///   Represents a system that handles the scheduling of the scripts in a game.
    /// </summary>
    public sealed class ScriptSystem : GameSystemBase
    {
        private const long UpdateBit = 1L << 32;

        internal static readonly Logger Log = GlobalLogger.GetLogger("ScriptSystem");

        /// <summary>
        ///   Contains all currently executed scripts.
        /// </summary>
        private readonly HashSet<ScriptComponent> registeredScripts = new HashSet<ScriptComponent>();
        private readonly HashSet<ScriptComponent> scriptsToStart = new HashSet<ScriptComponent>();
        private readonly HashSet<SyncScript> syncScripts = new HashSet<SyncScript>();
        private readonly List<ScriptComponent> scriptsToStartCopy = new List<ScriptComponent>();
        private readonly List<SyncScript> syncScriptsCopy = new List<SyncScript>();

        /// <summary>
        ///   Gets the scheduler that manages the execution of the scripts.
        /// </summary>
        /// <value>The scheduler.</value>
        public Scheduler Scheduler { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ScriptSystem"/> class.
        /// </summary>
        /// <param name="registry">The service registry.</param>
        /// <remarks>
        ///   The script system is expecting the following services to be registered: <see cref="IGame"/> and <see cref="ContentManager"/>.
        /// </remarks>
        public ScriptSystem(IServiceRegistry registry)
            : base(registry)
        {
            Enabled = true;
            Scheduler = new Scheduler();
            Scheduler.ActionException += Scheduler_ActionException;
        }

        protected override void Destroy()
        {
            Scheduler.ActionException -= Scheduler_ActionException;
            Scheduler = null;

            Services.RemoveService<ScriptSystem>();

            base.Destroy();
        }

        public override void Update(GameTime gameTime)
        {
            // Copy scripts to process (so that scripts added during this frame don't affect us)
            // TODO: How to handle scripts that we want to start during current frame?
            scriptsToStartCopy.AddRange(scriptsToStart);
            scriptsToStart.Clear();
            syncScriptsCopy.AddRange(syncScripts);

            // Schedule new scripts: StartupScript.Start() and AsyncScript.Execute()
            foreach (var script in scriptsToStartCopy)
            {
                // Start the script
                if (script is StartupScript startupScript)
                {
                    startupScript.StartSchedulerNode = Scheduler.Add(startupScript.Start, startupScript.Priority, startupScript, startupScript.ProfilingKey);
                }
                else
                {
                    // Start a microthread with execute method if it's an async script
                    if (script is AsyncScript asyncScript)
                    {
                        asyncScript.MicroThread = AddTask(asyncScript.Execute, asyncScript.Priority | UpdateBit);
                        asyncScript.MicroThread.ProfilingKey = asyncScript.ProfilingKey;
                    }
                }
            }

            // Schedule existing scripts: SyncScript.Update()
            foreach (var syncScript in syncScriptsCopy)
            {
                // Update priority
                var updateSchedulerNode = syncScript.UpdateSchedulerNode;
                updateSchedulerNode.Value.Priority = syncScript.Priority | UpdateBit;

                // Schedule
                Scheduler.Schedule(updateSchedulerNode, ScheduleMode.Last);
            }

            // Run current micro threads
            Scheduler.Run();

            // Flag scripts as not being live reloaded after starting/executing them for the first time
            foreach (var script in scriptsToStartCopy)
            {
                // Remove the start node after it got executed
                if (script is StartupScript startupScript)
                {
                    startupScript.StartSchedulerNode = null;
                }

                if (script.IsLiveReloading)
                    script.IsLiveReloading = false;
            }

            scriptsToStartCopy.Clear();
            syncScriptsCopy.Clear();
        }

        /// <summary>
        ///   Allows to wait for next frame.
        /// </summary>
        /// <returns>An awaiter that can be used to wait for the next frame.</returns>
        public ChannelMicroThreadAwaiter<int> NextFrame()
        {
            return Scheduler.NextFrame();
        }

        /// <summary>
        ///   Adds the specified microthread function.
        /// </summary>
        /// <param name="microThreadFunction">The microthread function.</param>
        /// <param name="priority">Priority for the scheduling of the function.</param>
        /// <returns>A microthread encapsulating the execution of the provided function.</returns>
        public MicroThread AddTask(Func<Task> microThreadFunction, long priority = 0)
        {
            var microThread = Scheduler.Create();
            microThread.Priority = priority;
            microThread.Start(microThreadFunction);
            return microThread;
        }

        /// <summary>
        ///   Waits for when all the specified microthreads have ran their tasks to completion.
        /// </summary>
        /// <param name="microThreads">The microthreads to await.</param>
        /// <returns>A task that can be awaited.</returns>
        public async Task WhenAll(params MicroThread[] microThreads)
        {
            await Scheduler.WhenAll(microThreads);
        }

        /// <summary>
        ///   Add the provided script to the script system.
        /// </summary>
        /// <param name="script">The script to add.</param>
        public void Add(ScriptComponent script)
        {
            script.Initialize(Services);
            registeredScripts.Add(script);

            // Register script for Start() and possibly async Execute()
            scriptsToStart.Add(script);

            // If it's a synchronous script, add it to the list as well
            if (script is SyncScript syncScript)
            {
                syncScript.UpdateSchedulerNode = Scheduler.Create(syncScript.Update, syncScript.Priority | UpdateBit);
                syncScript.UpdateSchedulerNode.Value.Token = syncScript;
                syncScript.UpdateSchedulerNode.Value.ProfilingKey = syncScript.ProfilingKey;
                syncScripts.Add(syncScript);
            }
        }

        /// <summary>
        ///   Remove the provided script from the script system.
        /// </summary>
        /// <param name="script">The script to remove.</param>
        public void Remove(ScriptComponent script)
        {
            // Make sure it's not registered in any pending list
            var startWasPending = scriptsToStart.Remove(script);
            var wasRegistered = registeredScripts.Remove(script);

            if (!startWasPending && wasRegistered)
            {
                // Cancel scripts that were already started
                try
                {
                    script.Cancel();
                }
                catch (Exception ex)
                {
                    HandleSynchronousException(script, ex);
                }

                var asyncScript = script as AsyncScript;
                asyncScript?.MicroThread.Cancel();
            }

            // Remove script from the scheduler, in case it was removed during scheduler execution
            if (script is StartupScript startupScript)
            {
                if (startupScript.StartSchedulerNode != null)
                {
                    Scheduler?.Unschedule(startupScript.StartSchedulerNode);
                    startupScript.StartSchedulerNode = null;
                }

                if (script is SyncScript syncScript)
                {
                    syncScripts.Remove(syncScript);
                    Scheduler?.Unschedule(syncScript.UpdateSchedulerNode);
                    syncScript.UpdateSchedulerNode = null;
                }
            }
        }

        /// <summary>
        ///   Method called by a live scripting debugger to notify the script system about reloaded scripts.
        /// </summary>
        /// <param name="oldScript">The old script.</param>
        /// <param name="newScript">The new script.</param>
        public void LiveReload(ScriptComponent oldScript, ScriptComponent newScript)
        {
            // Set live reloading mode for the rest of it's lifetime
            oldScript.IsLiveReloading = true;

            // Set live reloading mode until after being started
            newScript.IsLiveReloading = true;
        }

        private void Scheduler_ActionException(Scheduler scheduler, SchedulerEntry schedulerEntry, Exception e)
        {
            HandleSynchronousException((ScriptComponent) schedulerEntry.Token, e);
        }

        private void HandleSynchronousException(ScriptComponent script, Exception e)
        {
            Log.Error("Unexpected exception while executing a script.", e);

            // Only crash if live scripting debugger is not listening
            if (Scheduler.PropagateExceptions)
                ExceptionDispatchInfo.Capture(e).Throw();

            // Remove script from all lists
            if (script is SyncScript syncScript)
            {
                syncScripts.Remove(syncScript);
            }

            registeredScripts.Remove(script);
        }

        private class PriorityScriptComparer : IComparer<ScriptComponent>
        {
            public static readonly PriorityScriptComparer Default = new PriorityScriptComparer();

            public int Compare(ScriptComponent x, ScriptComponent y)
            {
                return x.Priority.CompareTo(y.Priority);
            }
        }
    }
}
