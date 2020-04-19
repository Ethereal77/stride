// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Assets.Presentation.AssetEditors.GameEditor;
using Xenko.Editor.EditorGame.ViewModels;

namespace Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
{
    public interface IEditorGameRenderModeViewModelService : IEditorGameViewModelService
    {
        /// <summary>
        /// Gets or sets the material filter stream.
        /// </summary>
        /// <value>The material filter stream.</value>
        EditorRenderMode RenderMode { get; set; }
    }
}
