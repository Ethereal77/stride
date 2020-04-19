// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;
using Xenko.Core.Presentation.Quantum;
using Xenko.Core.Presentation.Quantum.View;
using Xenko.Core.Presentation.Quantum.ViewModels;

namespace Xenko.Assets.Presentation.TemplateProviders
{
    public class EntityComponentReferenceTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => "EntityComponentReference";

        public override bool MatchNode(NodeViewModel node)
        {
            return typeof(EntityComponent).IsAssignableFrom(node.Type) && node.Parent?.Type != typeof(EntityComponentCollection);
        }
    }
}
