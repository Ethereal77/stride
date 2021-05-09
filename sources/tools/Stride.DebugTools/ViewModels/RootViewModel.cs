// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Framework.Time;
using Stride.Core.Presentation.Extensions;
using System.Collections.ObjectModel;

using Stride.Framework.MicroThreading;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Stride.Framework.Diagnostics;
using Stride.DebugTools.DataStructures;
using Stride.Core.Presentation;
using Stride.Core.Presentation.Commands;

namespace Stride.DebugTools.ViewModels
{
    public class RootViewModel : DeprecatedViewModelBase
    {
        public EngineContext EngineContext { get; private set; }
        public ICommand SnapshotCommand { get; private set; }

        private readonly MicroThreadMonitoringManager microThreadMonitoringManager;
        private IDisposable subscription;

        private readonly ProcessInfoRenderer processInfoRenderer;

        //public ICommand PauseCommand { get; private set; }
        //public ICommand ResumeCommand { get; private set; }
        //public ICommand NextFrameCommand { get; private set; }

        private int increment;
        public int Increment
        {
            get => increment;

            set
            {
                increment = value;
                OnPropertyChanged(nameof(Increment));
            }
        }

        private ICommand testBehavior;
        public ICommand TestBehavior
        {
            get
            {
                if (testBehavior is null)
                    testBehavior = new AnonymousCommand(prm => MessageBox.Show(string.Format("CommandParameter: {0}", prm)));
                return testBehavior;
            }
        }

        private DispatcherTimer timer;

        public RootViewModel(EngineContext engineContext, ProcessInfoRenderer processInfoRenderer)
        {
            if (engineContext is null)
                throw new ArgumentNullException(nameof(engineContext));

            if (engineContext.Scheduler is null)
                throw new InvalidOperationException("The provided EngineContext must have a Scheduler.");

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0),
                IsEnabled = true,
            };
            timer.Tick += (s, e) => Increment++;
            timer.Start();

            this.processInfoRenderer = processInfoRenderer;

            EngineContext = engineContext;
            microThreadMonitoringManager = new MicroThreadMonitoringManager(EngineContext.Scheduler);

            SnapshotCommand = new AnonymousCommand(_ => TakeSnapshot());

            //PauseCommand = new AnonymousCommand(_ => EngineContext.Scheduler.PauseExecution());
            //ResumeCommand = new AnonymousCommand(_ => EngineContext.Scheduler.ResumeExecution());
            //NextFrameCommand = new AnonymousCommand(_ => EngineContext.Scheduler.NextExecutionFrame());

            SetupMicroThreadWatching();
            SetupScriptWatching();

            StartMonitoring();
        }

        private readonly ObservableCollection<MicroThreadViewModel> microThreads = new ObservableCollection<MicroThreadViewModel>();
        public IEnumerable<MicroThreadViewModel> MicroThreads { get { return microThreads; } }

        private void SetupMicroThreadWatching()
        {
            if (EngineContext.Scheduler is null || EngineContext.Scheduler.MicroThreads is null)
                return;

            foreach (MicroThread mt in EngineContext.Scheduler.MicroThreads)
                microThreads.Add(new MicroThreadViewModel(mt));

            EngineContext.Scheduler.MicroThreadCreated += (s, e) => AddMicroThread(e.MicroThread);
            EngineContext.Scheduler.MicroThreadDeleted += (s, e) => RemoveMicroThread(e.MicroThread.Id);
        }

        private void AddMicroThread(MicroThread microThread)
        {
            Dispatcher.BeginInvoke(delegate
            {
                microThreads.Add(new MicroThreadViewModel(microThread));
            }, null);
        }

        private void RemoveMicroThread(long microThreadId)
        {
            Dispatcher.BeginInvoke(delegate
            {
                MicroThreadViewModel foundViewModel = microThreads.FirstOrDefault(vm => vm.Id == microThreadId);
                if (foundViewModel != null)
                    microThreads.Remove(foundViewModel);
            }, null);
        }

        private readonly ObservableCollection<ScriptAssemblyViewModel> scriptAssemblies = new ObservableCollection<ScriptAssemblyViewModel>();
        public IEnumerable<ScriptAssemblyViewModel> ScriptAssemblies => scriptAssemblies;

        private ScriptAssemblyViewModel orphanScriptsAssembly;

        private void SetupScriptWatching()
        {
            if (EngineContext.ScriptManager is null)
                return;

            foreach (ScriptAssembly scriptAssembly in EngineContext.ScriptManager.ScriptAssemblies)
            {
                AddScriptAssembly(scriptAssembly);
            }

            List<ScriptEntry2> orphanScripts = EngineContext.ScriptManager.Scripts
                .Where(s => s.Assembly is null)
                .ToList();

            if (orphanScripts.Count > 0)
                AddScriptAssembly(new ScriptAssembly { Scripts = orphanScripts });

            EngineContext.ScriptManager.ScriptAssemblyAdded += (s, e) => AddScriptAssembly(e.ScriptAssembly);
            EngineContext.ScriptManager.ScriptAssemblyRemoved += (s, e) => RemoveScriptAssembly(e.ScriptAssembly);
        }

        private void AddScriptAssembly(ScriptAssembly scriptAssembly)
        {
            ScriptAssemblyViewModel vm;
            if (scriptAssembly.Assembly is null)
            {
                if (orphanScriptsAssembly is null)
                {
                    orphanScriptsAssembly = new ScriptAssemblyViewModel(scriptAssembly, this);
                    scriptAssemblies.Add(orphanScriptsAssembly);
                }
                vm = orphanScriptsAssembly;
            }
            else
            {
                vm = new ScriptAssemblyViewModel(scriptAssembly, this);
                scriptAssemblies.Add(vm);
            }

            vm.UpdateScripts();
        }

        private void RemoveScriptAssembly(ScriptAssembly scriptAssembly)
        {
            ScriptAssemblyViewModel foundViewModel = scriptAssemblies.Single(item => string.Compare(item.Url, scriptAssembly.Url, true) == 0);
            scriptAssemblies.Remove(foundViewModel);
        }

        private void TakeSnapshot()
        {
            ProcessInfo processInfo = microThreadMonitoringManager.TakeProcessInfoDataSnapshot();

            var snapshotWindow = new Window
            {
                Content = new ProcessSnapshotControl(processInfo)
            };

            snapshotWindow.Show();
        }

        private void RenderProcessData()
        {
            processInfoRenderer.RenderLastFrame(microThreadMonitoringManager.TakeProcessInfoDataSnapshot());
        }

        public void StartMonitoring()
        {
            subscription = microThreadMonitoringManager
                .ObserveOn(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher))
                .Subscribe(_ => RenderProcessData());

            microThreadMonitoringManager.StartMonitoring();
        }

        public void StopMonitoring()
        {
            subscription.Dispose();
            microThreadMonitoringManager.StopMonitoring();
        }
    }
}
