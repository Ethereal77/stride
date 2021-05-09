// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Presentation.Preview;
using Stride.Editor.Preview;
using Stride.Editor.Preview.ViewModel;

namespace Stride.Assets.Presentation.ViewModel.Preview
{
    [AssetPreviewViewModel(typeof(MaterialPreview))]
    public class MaterialPreviewViewModel : AssetPreviewViewModel
    {
        private MaterialPreview materialPreview;
        private MaterialPreviewPrimitive selectedPrimitive;

        public MaterialPreviewViewModel(SessionViewModel session)
            : base(session)
        {
        }

        public Type PrimitiveTypes => typeof(MaterialPreviewPrimitive);
        
        public MaterialPreviewPrimitive SelectedPrimitive { get { return selectedPrimitive; } set { SetValue(ref selectedPrimitive, value); SetPrimitive(value); } }

        public override void AttachPreview(IAssetPreview preview)
        {
            this.materialPreview = (MaterialPreview)preview;
            SetPrimitive(SelectedPrimitive);
        }

        private void SetPrimitive(MaterialPreviewPrimitive primitive)
        {
            materialPreview.SetPrimitive(primitive);
        }
    }
}
