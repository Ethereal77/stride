// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Editor.Extensions;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Skyboxes;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.ViewModel;
using Stride.Core.Quantum;

namespace Stride.Assets.Presentation.ViewModel
{
    [AssetViewModel(typeof(SkyboxAsset))]
    public class SkyboxViewModel : AssetViewModel<SkyboxAsset>
    {
        public SkyboxViewModel(AssetViewModelConstructionParameters parameters)
            : base(parameters)
        {
        }
    }
}
