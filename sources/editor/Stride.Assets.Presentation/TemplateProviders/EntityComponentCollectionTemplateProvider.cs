// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Engine;
using Xenko.Core.Presentation.Quantum;
using Xenko.Core.Presentation.Quantum.View;
using Xenko.Core.Presentation.Quantum.ViewModels;

namespace Xenko.Assets.Presentation.TemplateProviders
{
    public class EntityComponentCollectionTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => "EntityComponentCollection";

        public override bool MatchNode(NodeViewModel node)
        {
            return node.Type == typeof(EntityComponentCollection) && node.Parent?.Type == typeof(Entity);
        }
    }
}
