// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Effect;

namespace Stride.Assets.Presentation.ViewModel
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
