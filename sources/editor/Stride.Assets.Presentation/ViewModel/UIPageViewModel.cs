// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Annotations;
using Stride.Assets.UI;

namespace Stride.Assets.Presentation.ViewModel
{
    /// <summary>
    /// View model for <see cref="UIPageAsset"/>.
    /// </summary>
    [AssetViewModel(typeof(UIPageAsset))]
    public class UIPageViewModel : UIBaseViewModel, IAssetViewModel<UIPageAsset>
    {
        public UIPageViewModel([NotNull] AssetViewModelConstructionParameters parameters)
            : base(parameters)
        {

        }

        /// <inheritdoc />
        public new UIPageAsset Asset => (UIPageAsset)base.Asset;
    }
}
