// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Editor.EditorGame.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
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
