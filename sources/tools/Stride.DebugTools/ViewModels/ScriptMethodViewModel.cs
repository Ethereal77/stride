// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Framework.MicroThreading;
using Stride.Extensions;
using System.Windows.Input;

using Stride.Core.Presentation;
using Stride.Core.Presentation.Commands;

namespace Stride.DebugTools.ViewModels
{
    public class ScriptMethodViewModel : DeprecatedViewModelBase
    {
        public ScriptAssemblyViewModel AssemblyParent { get; private set; }
        public ScriptTypeViewModel TypeParent { get; private set; }

        public ICommand RunScriptCommand { get; private set; }
        public ICommand CloseMicroThreadView { get; private set; }

        private ScriptEntry2 scriptEntry;

        public ScriptMethodViewModel(ScriptEntry2 scriptEntry, ScriptTypeViewModel parent)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            TypeParent = parent;
            AssemblyParent = TypeParent.Parent;

            this.scriptEntry = scriptEntry;

            RunScriptCommand = new AnonymousCommand(_ => OnRunScriptCommand());
            CloseMicroThreadView = new AnonymousCommand(_ => MicroThread = null);
        }

        public string Name => scriptEntry.MethodName;

        public bool IsAssemblyStartup => scriptEntry.Flags == ScriptFlags.AssemblyStartup;

        public bool HasNoFlags => scriptEntry.Flags == ScriptFlags.None;

        public ScriptFlags FlagsDisplay => scriptEntry.Flags;

        private void OnRunScriptCommand()
        {
            MicroThread mt = AssemblyParent.Parent.EngineContext.ScriptManager.RunScript(scriptEntry, null);
            MicroThread = new MicroThreadViewModel(mt);
        }

        private MicroThreadViewModel microThread;
        public MicroThreadViewModel MicroThread
        {
            get => microThread;

            private set
            {
                if (microThread != value)
                {
                    microThread = value;
                    OnPropertyChanged(nameof(MicroThread));
                }
            }
        }
    }
}
