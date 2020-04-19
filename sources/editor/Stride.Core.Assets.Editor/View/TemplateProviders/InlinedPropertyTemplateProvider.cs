// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Quantum.NodePresenters.Keys;
using Xenko.Core.Presentation.Quantum;
using Xenko.Core.Presentation.Quantum.View;
using Xenko.Core.Presentation.Quantum.ViewModels;

namespace Xenko.Core.Assets.Editor.View.TemplateProviders
{
    public class InlinedPropertyTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => "InlinedProperty";

        public override bool MatchNode(NodeViewModel node)
        {
            object inlined;
            return node.AssociatedData.TryGetValue(InlineData.InlineMember, out inlined) && (bool)inlined;
        }
    }
}
