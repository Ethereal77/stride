// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Assets.Effect;

namespace Xenko.Assets.Presentation.ViewModel
{
    [AssetViewModel(typeof(EffectLogAsset))]
    public class EffectLogViewModel : AssetViewModel<EffectLogAsset>
    {
        public EffectLogViewModel(AssetViewModelConstructionParameters parameters) : base(parameters)
        {
        }

        public string Text
        {
            get { return Asset.Text; }
            set { SetValue(Asset.Text != value, () => Asset.Text = value); }
        }
    }
}
