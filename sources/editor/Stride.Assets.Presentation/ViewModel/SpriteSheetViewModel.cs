// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Extensions;
using Stride.Assets.Presentation.ViewModel.Commands;
using Stride.Assets.Sprite;
using Stride.Core.Presentation.Quantum;
using Stride.Core.Presentation.Quantum.Presenters;
using Stride.Assets.Presentation.AssetEditors.SpriteEditor.ViewModels;
using Stride.Assets.Presentation.NodePresenters.Commands;

namespace Stride.Assets.Presentation.ViewModel
{
    [AssetViewModel(typeof(SpriteSheetAsset))]
    public class SpriteSheetViewModel : AssetViewModel<SpriteSheetAsset>
    {
        internal new SpriteSheetEditorViewModel Editor => (SpriteSheetEditorViewModel)base.Editor;

        public SpriteSheetViewModel(AssetViewModelConstructionParameters parameters)
            : base(parameters)
        {
        }
    }
}
