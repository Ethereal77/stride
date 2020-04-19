// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Views;
using Xenko.Assets.Presentation.AssetEditors.SceneEditor.ViewModels;
using Xenko.Assets.Presentation.ViewModel;

namespace Xenko.Assets.Presentation.AssetEditors.SceneEditor.Views
{
    public class SceneEditorView : EntityHierarchyEditorView
    {
        /// <inheritdoc />
        protected override EntityHierarchyEditorViewModel CreateEditorViewModel(AssetViewModel asset)
        {
            return SceneEditorViewModel.Create((SceneViewModel)asset);
        }
    }
}
