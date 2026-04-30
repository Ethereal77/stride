// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Core.Assets.Editor.View.TemplateProviders
{
    public class ArrayTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => (ElementType?.Name ?? "") + "[]";

        public Type ElementType { get; set; }

        public override bool MatchNode(NodeViewModel node)
        {
            if (node.Type.IsArray)
            {
                return node.NodeValue != null;
            }
            return false;
        }
    }
}
