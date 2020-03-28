// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xenko.Core.Assets;
using Xenko.Core.Assets.Templates;

namespace Xenko.Assets.Presentation.Templates
{
    public class GraphicsCompositorTemplateGenerator : AssetFactoryTemplateGenerator
    {
        public new static readonly GraphicsCompositorTemplateGenerator Default = new GraphicsCompositorTemplateGenerator();

        public static readonly Dictionary<Guid, string> SupportedTemplatesToUrl = new Dictionary<Guid, string>
        {
            { new Guid("D4EE3BD3-9B06-460E-9175-D6AFB2459463"), XenkoPackageUpgrader.DefaultGraphicsCompositorLevel10Url },
            { new Guid("4BC182D7-69D5-4BE2-9AF3-1C82F67B629D"), "GraphicsCompositor/DefaultGraphicsCompositorVoxels" },
        };

        public override bool IsSupportingTemplate(TemplateDescription templateDescription)
        {
            if (templateDescription == null) throw new ArgumentNullException(nameof(templateDescription));
            return SupportedTemplatesToUrl.ContainsKey(templateDescription.Id);
        }

        protected override IEnumerable<AssetItem> CreateAssets(AssetTemplateGeneratorParameters parameters)
        {
            // Find default graphics compositor to create a derived asset from
            var graphicsCompositor = SupportedTemplatesToUrl.TryGetValue(parameters.Description.Id, out var graphicsCompositorUrl) ? parameters.Package.FindAsset(graphicsCompositorUrl) : null;

            // Something went wrong, create an empty asset
            if (graphicsCompositor == null)
                return base.CreateAssets(parameters);

            // Create derived asset
            return new[] { new AssetItem(GenerateLocation(parameters), graphicsCompositor.CreateDerivedAsset()) };
        }
    }
}
