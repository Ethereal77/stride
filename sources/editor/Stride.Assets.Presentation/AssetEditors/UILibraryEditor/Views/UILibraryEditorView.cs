// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Editor.ViewModel;
using Stride.Assets.Presentation.AssetEditors.UIEditor.ViewModels;
using Stride.Assets.Presentation.AssetEditors.UIEditor.Views;
using Stride.Assets.Presentation.AssetEditors.UILibraryEditor.ViewModels;
using Stride.Assets.Presentation.ViewModel;

namespace Stride.Assets.Presentation.AssetEditors.UILibraryEditor.Views
{
    public class UILibraryEditorView : UIEditorView
    {
        protected override UIEditorBaseViewModel CreateEditorViewModel(AssetViewModel asset)
        {
            return UILibraryEditorViewModel.Create((UILibraryViewModel)asset);
        }
    }
}
