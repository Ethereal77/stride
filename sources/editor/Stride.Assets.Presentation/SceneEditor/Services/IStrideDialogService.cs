// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;

namespace Stride.Assets.Presentation.SceneEditor.Services
{
    /// <summary>
    /// This interface represents Stride-specifc dialog service.
    /// </summary>
    public interface IStrideDialogService
    {
        /// <summary>
        /// Creates an entity picker dialog.
        /// </summary>
        /// <param name="editor">The editor view model currently in use.</param>
        /// <returns>An instance of the <see cref="IEntityPickerDialog"/> interface.</returns>
        IEntityPickerDialog CreateEntityPickerDialog(EntityHierarchyEditorViewModel editor);

        /// <summary>
        /// Creates an entity component picker dialog.
        /// </summary>
        /// <param name="editor">The editor view model currently in use.</param>
        /// <param name="componentType">The type of component to pickup.</param>
        /// <returns>An instance of the <see cref="IEntityPickerDialog"/> interface.</returns>
        IEntityPickerDialog CreateEntityComponentPickerDialog(EntityHierarchyEditorViewModel editor, Type componentType);
    }
}
