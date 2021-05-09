// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Assets.SpriteFont;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;

namespace Stride.Assets.Presentation.TemplateProviders
{
    public class SpriteFontFontNamePropertyTemplateProvider : NodeViewModelTemplateProvider
    {
        public static string FontNamePropertyName = nameof(SystemFontProvider.FontName);

        public override string Name => "SpriteFontFontName";

        public override bool MatchNode(NodeViewModel node)
        {
            if (!typeof(SpriteFontAsset).IsAssignableFrom(node.Root.Type))
                return false;

            return node.Name == FontNamePropertyName;
        }
    }
}
