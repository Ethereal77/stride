// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.View.TemplateProviders;
using Xenko.Engine;
using Xenko.Core.Presentation.Quantum;
using Xenko.Core.Presentation.Quantum.ViewModels;
using Xenko.Rendering;

namespace Xenko.Assets.Presentation.TemplateProviders
{
    public class ModelComponentMaterialTemplateProvider : ContentReferenceTemplateProvider
    {
        public ModelComponentMaterialTemplateProvider()
        {
            DynamicThumbnail = true;
        }

        public override string Name => "ModelComponentMaterial";

        public override bool MatchNode(NodeViewModel node)
        {
            return base.MatchNode(node)
                && node.NodeValue is Material
                && node.Parent?.Parent?.NodeValue is ModelComponent;
        }
    }
}
