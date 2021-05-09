// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Framework.MicroThreading;
using Stride.Extensions;
using System.Windows.Input;

using Stride.Core.Presentation.Commands;
using Stride.Core.Presentation;

namespace Stride.DebugTools.ViewModels
{
    public class ScriptTypeViewModel : DeprecatedViewModelBase
    {
        public ScriptAssemblyViewModel Parent { get; private set; }
        public IEnumerable<ScriptMethodViewModel> Methods { get; private set; }

        private readonly string typeName;

        public ScriptTypeViewModel(string typeName, IEnumerable<ScriptEntry2> scriptEntries, ScriptAssemblyViewModel parent)
        {
            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));

            if (scriptEntries is null)
                throw new ArgumentNullException(nameof(scriptEntries));

            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            Parent = parent;
            this.typeName = typeName;

            Methods = scriptEntries.Select(item => new ScriptMethodViewModel(item, this));
        }

        public string[] Namespaces
        {
            get
            {
                string ns = Namespace;
                return ns != null ? ns.Split('.') : null;
            }
        }

        public string Namespace
        {
            get
            {
                string[] elements = typeName.Split('.');

                if (elements.Length == 1)
                    return null; // Namespace-less type

                return string.Join(".", elements.Take(elements.Length - 1));
            }
        }

        public string[] TypeNames => TypeName.Split('+'); // To get nested types

        public string TypeName => typeName.Split('.').Last();

        public string FullName => typeName;
    }
}
