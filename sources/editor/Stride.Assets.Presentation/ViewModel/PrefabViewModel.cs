// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Annotations;
using Stride.Core.Quantum;
using Stride.Assets.Entities;

namespace Stride.Assets.Presentation.ViewModel
{
    [AssetViewModel(typeof(PrefabAsset))]
    public class PrefabViewModel : EntityHierarchyViewModel, IAssetViewModel<PrefabAsset>
    {
        public PrefabViewModel([NotNull] AssetViewModelConstructionParameters parameters)
            : base(parameters)
        {
            
        }

        /// <inheritdoc />
        public new PrefabAsset Asset => (PrefabAsset)base.Asset;

        protected override IObjectNode GetPropertiesRootNode()
        {
            // We don't use CanProvideProperties because we still want the button to open in editor. But we don't want to display any property directly
            return null;
        }
    }
}
