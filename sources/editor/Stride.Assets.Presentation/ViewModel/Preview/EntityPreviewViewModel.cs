// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Presentation.Preview;
using Stride.Editor.Preview;
using Stride.Editor.Preview.ViewModel;

namespace Stride.Assets.Presentation.ViewModel.Preview
{
    [AssetPreviewViewModel(typeof(EntityPreview))]
    public class EntityPreviewViewModel : AssetPreviewViewModel
    {
        public EntityPreviewViewModel(SessionViewModel session)
            : base(session)
        {
        }

        public override void AttachPreview(IAssetPreview preview)
        {
            // Nothing for now
        }
    }
}
