// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Assets.Presentation.AssetEditors.GameEditor;
using Stride.Editor.EditorGame.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
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
