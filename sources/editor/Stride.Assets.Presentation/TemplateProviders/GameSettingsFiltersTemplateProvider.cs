// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.View;
using Stride.Core.Presentation.Quantum.ViewModels;
using Stride.Assets.Presentation.ViewModel;

namespace Stride.Assets.Presentation.TemplateProviders
{
    public class GameSettingsFiltersTemplateProvider : NodeViewModelTemplateProvider
    {
        public override string Name => nameof(GameSettingsFiltersTemplateProvider);

        public override bool MatchNode(NodeViewModel node)
        {
            return node.Name == "SpecificFilter" && node.Root.Type == typeof(GameSettingsAsset);
        }
    }
}
