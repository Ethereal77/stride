// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Framework;
using Stride.Framework.MicroThreading;
using Stride.Core.Extensions;
using System.Threading;

using Stride.Framework.Time;
using Stride.DebugTools.DataStructures;
using Stride.Core.Presentation.Observable;

namespace Stride.DebugTools
{
    /// <summary>
    ///   Manager class that monitors the microthreads executions.
    /// </summary>
    /// <remarks>
    ///   This class registers to <c>Scheduler</c> events and listen to <c>MicroThread</c>s state changes.
    ///   It provides monitoring information through <c>IObservable</c> notifications.
    /// </remarks>
    public class MicroThreadMonitoringManager : IObservable<FrameInfo>
    {
        /// <summary>
        ///   Maximum number of microthreads execution frames stored by the monitoring manager.
        /// </summary>
        public const int MaximumCapturedFrames = 30;

        public Scheduler Scheduler { get; private set; }
        public uint CurrentFrameNumber => frameNumber;

        private readonly AbsoluteStopwatch stopwatch = new AbsoluteStopwatch();
        private readonly ObserverContainer<FrameInfo> observerContainer = new ObserverContainer<FrameInfo>();

        private readonly object processInfoLock = new object();
        private ProcessInfo processInfo = new ProcessInfo();
        private FrameInfo frameInfo;
        private uint frameNumber;

        private readonly Dictionary<long, MicroThreadPendingState> pendingMicroThreads = new Dictionary<long, MicroThreadPendingState>();

        public MicroThreadMonitoringManager(Scheduler scheduler)
        {
            if (scheduler is null)
                throw new ArgumentNullException(nameof(scheduler));

            Scheduler = scheduler;
        }

        /// <summary>
        ///   Registers an observer for further <see cref="FrameInfo"/> notifications.
        /// </summary>
        /// <param name="observer">Observer that will receive outgoing notifications.</param>
        /// <returns>Returns the subscription token.</returns>
        public IDisposable Subscribe(IObserver<FrameInfo> observer)
        {
            return observerContainer.Subscribe(observer);
        }

        /// <summary>
        ///   Starts the microthreads monitoring.
        /// </summary>
        /// <remarks>
        ///   This method resets the time and the frame number.
        /// </remarks>
        public void StartMonitoring()
        {
            frameNumber = 0;
            // New MicroThread system doesn't have any PropertyChanged event yet
            throw new NotImplementedException();
            //Scheduler.MicroThreadStateChanged += OnMicroThreadStateChanged;
            //Scheduler.MessageLoopBegin += OnMessageLoopBegin;
            stopwatch.Start();
        }

        /// <summary>
        ///   Stops the microthreads monitoring.
        /// </summary>
        public void StopMonitoring()
        {
            // New MicroThread system doesn't have any PropertyChanged event yet
            throw new NotImplementedException();
            //Scheduler.MessageLoopBegin -= OnMessageLoopBegin;
            //Scheduler.MicroThreadStateChanged -= OnMicroThreadStateChanged;
        }

        /// <summary>
        ///   Deletes all the previously monitored frame data.
        /// </summary>
        /// <remarks>
        ///   This method also resets time and the frame number.
        /// </remarks>
        public void ClearMonitoringData()
        {
            frameNumber = 0;
            lock (processInfoLock)
            {
                processInfo = new ProcessInfo();
            }
            GC.Collect();
            stopwatch.Start();
        }

        /// <summary>
        ///   Retrieves the already monitored and stored data.
        /// </summary>
        /// <remarks>
        ///   Please be careful while using this data as it is not thread-safe.
        /// </remarks>
        public ProcessInfo GetProcessInfoData()
        {
            return processInfo;
        }

        /// <summary>
        ///   Takes a snapshot of the monitored data.
        /// </summary>
        /// <remarks>
        ///   This set of data is acquired in a thread-safe manner.
        /// </remarks>
        public ProcessInfo TakeProcessInfoDataSnapshot()
        {
            lock (processInfoLock)
            {
                return processInfo.Duplicate();
            }
        }

        private void OnMicroThreadStateChanged(object sender, SchedulerEventArgs e)
        {
            if (e.MicroThreadPreviousState == MicroThreadState.None)
                return;

            if (frameInfo is null)
                return;

            double currentTime = stopwatch.Elapsed;

            int threadId = Thread.CurrentThread.ManagedThreadId;
            long microThreadId = e.MicroThread.Id;

            ThreadInfo threadInfo = frameInfo.ThreadItems.FirstOrDefault(ti => ti.Id == threadId);
            if (threadInfo is null)
            {
                threadInfo = new ThreadInfo { Id = threadId };
                frameInfo.ThreadItems.Add(threadInfo);
            }

            // Pending state is used to keep trace of the microthreads recently added as 'Running'
            // In order to create a proper MicroThreadInfo item when then receiving a 'Waiting' notification
            if (pendingMicroThreads.TryGetValue(microThreadId, out MicroThreadPendingState pendingState))
            {
                threadInfo.MicroThreadItems.Add(new MicroThreadInfo
                {
                    Id = microThreadId,
                    BeginState = pendingState.State,
                    EndState = e.MicroThread.State,
                    BeginTime = Math.Max(pendingState.Time, frameInfo.BeginTime),
                    EndTime = currentTime,
                });

                pendingMicroThreads.Remove(microThreadId);
            }
            else if (e.MicroThread.IsOver == false)
            {
                pendingMicroThreads.Add(microThreadId, new MicroThreadPendingState
                {
                    ThreadId = threadInfo.Id,
                    Time = currentTime,
                    State = e.MicroThread.State,
                    MicroThread = e.MicroThread,
                });
            }
        }

        private void OnMessageLoopBegin(object sender, EventArgs e)
        {
            double currentTime = stopwatch.Elapsed;

            if (frameInfo != null)
            {
                frameInfo.EndTime = currentTime;

                foreach (MicroThreadPendingState pendingState in pendingMicroThreads.Values)
                {
                    ThreadInfo th = frameInfo.ThreadItems.FirstOrDefault(ti => ti.Id == pendingState.ThreadId);
                    if (th is null)
                        throw new InvalidOperationException($"Thread {pendingState.ThreadId} is not referenced in frame {frameInfo.FrameNumber}.");

                    th.MicroThreadItems.Add(new MicroThreadInfo
                    {
                        Id = pendingState.MicroThread.Id,
                        BeginState = pendingState.State,
                        EndState = pendingState.State,
                        BeginTime = Math.Max(pendingState.Time, frameInfo.BeginTime),
                        EndTime = currentTime,
                    });
                }

                // End of the previous frame
                lock (observerContainer.SyncRoot)
                {
                    observerContainer.Observers.ForEach(observer => observer.OnNext(frameInfo));
                }

                // Begining of the new frame
                lock (processInfoLock)
                {
                    processInfo.Frames.Add(frameInfo);
                }
            }

            frameInfo = new FrameInfo
            {
                BeginTime = currentTime,
                FrameNumber = frameNumber++,
            };

            lock (processInfoLock)
            {
                if (processInfo.Frames.Count > MaximumCapturedFrames)
                    processInfo.Frames.RemoveAt(0);
            }
        }
    }
}
