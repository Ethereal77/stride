// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.Quantum.NodePresenters.Keys;
using Xenko.Core.Reflection;
using Xenko.Core.Presentation.Quantum.View;
using Xenko.Core.Presentation.Quantum.ViewModels;

namespace Xenko.Core.Assets.Editor.View.TemplateProviders
{
    public class RangedValueTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => "RangedValueTemplateProvider";

        public override bool MatchNode(NodeViewModel node)
        {
            // We need at least a minimum and a maximum to display a slider, but we also rely on having explicit small and large steps to make sure that the
            // slider won't be between the whole integer range for instance.
            return node.Type.IsNumeric() && node.AssociatedData.ContainsKey(NumericData.Minimum) && node.AssociatedData.ContainsKey(NumericData.Maximum)
                   && node.AssociatedData.ContainsKey(NumericData.SmallStep) && node.AssociatedData.ContainsKey(NumericData.LargeStep);
        }
    }
}
