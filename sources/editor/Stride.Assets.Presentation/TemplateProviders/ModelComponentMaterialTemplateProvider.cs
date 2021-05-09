// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.View.TemplateProviders;
using Stride.Engine;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.ViewModels;
using Stride.Rendering;

namespace Stride.Assets.Presentation.TemplateProviders
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
