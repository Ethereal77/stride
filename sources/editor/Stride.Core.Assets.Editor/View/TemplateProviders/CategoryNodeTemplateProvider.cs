// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.Quantum.NodePresenters.Keys;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Core.Assets.Editor.View.TemplateProviders
{
    public class CategoryNodeTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => "CategoryNode";

        public override bool MatchNode(NodeViewModel node)
        {
            object value;
            if (node.AssociatedData.TryGetValue(CategoryData.Category, out value))
            {
                return (bool)value;
            }
            return false;
        }
    }
}
