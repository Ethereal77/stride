// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Core.Assets.Editor.View.TemplateProviders
{
    public class ArrayItemTemplateProvider : TypeMatchTemplateProvider
    {
        public override string Name => "ArrayItem";

        public override bool MatchNode(NodeViewModel node)
        {
            return base.MatchNode(node) && node.Parent != null && node.Parent.Type.IsArray;
        }
    }
}
