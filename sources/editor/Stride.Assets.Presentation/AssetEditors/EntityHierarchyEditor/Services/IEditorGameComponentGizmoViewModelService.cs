// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Editor.EditorGame.ViewModels;

namespace Xenko.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
{
    public interface IEditorGameComponentGizmoViewModelService : IEditorGameViewModelService
    {
        float GizmoSize { get; set; }

        bool FixedSize { get; set; }

        /// <summary>
        /// Changes the visibility of gizmos corresponding to a given component type.
        /// </summary>
        /// <param name="componentType">The component type of the gizmos.</param>
        /// <param name="isVisible">The value of visibility to set.</param>
        void ToggleGizmoVisibility(Type componentType, bool isVisible);
    }
}
