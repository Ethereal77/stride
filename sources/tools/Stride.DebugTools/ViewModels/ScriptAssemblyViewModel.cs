// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Presentation;

namespace Stride.DebugTools.ViewModels
{
    public class ScriptAssemblyViewModel : DeprecatedViewModelBase
    {
        public RootViewModel Parent { get; private set; }

        private readonly ScriptAssembly scriptAssembly;

        public ScriptAssemblyViewModel(ScriptAssembly scriptAssembly, RootViewModel parent)
        {
            if (scriptAssembly is null)
                throw new ArgumentNullException(nameof(scriptAssembly));

            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            Parent = parent;
            this.scriptAssembly = scriptAssembly;

            UpdateScripts();
        }

        public string Url => scriptAssembly.Url is null ?
            "<anonymous assembly>" :
            scriptAssembly.Url.TrimStart('/');

        public string Assembly => scriptAssembly.Assembly is null ? "-" : scriptAssembly.Assembly.ToString();

        public IEnumerable<ScriptTypeViewModel> Types { get; private set; }

        internal void UpdateScripts()
        {
            Types = from script in scriptAssembly.Scripts group script by script.TypeName into g select new ScriptTypeViewModel(g.Key, g, this);
            OnPropertyChanged<ScriptAssemblyViewModel>(n => n.Types);
        }
    }
}
