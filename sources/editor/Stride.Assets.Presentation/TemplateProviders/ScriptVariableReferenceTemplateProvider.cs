// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Reflection;

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;
using Stride.Assets.Presentation.NodePresenters.Keys;
using Stride.Assets.Presentation.NodePresenters.Updaters;
using Stride.Assets.Scripts;

namespace Stride.Assets.Presentation.TemplateProviders
{
    public class ScriptVariableReferenceTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => nameof(ScriptVariableReferenceTemplateProvider);

        public override bool MatchNode(NodeViewModel node)
        {
            return node.Type == typeof(string) && (node.Root?.AssociatedData.ContainsKey(VisualScriptData.OwnerBlock) ?? false) && node.MemberInfo?.GetCustomAttribute<ScriptVariableReferenceAttribute>() != null;
        }
    }
}
