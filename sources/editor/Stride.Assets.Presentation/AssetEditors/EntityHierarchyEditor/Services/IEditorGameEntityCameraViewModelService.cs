// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Assets.Presentation.AssetEditors.GameEditor.Services;
using Stride.Assets.Presentation.AssetEditors.GameEditor.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
{
    /// <summary>
    /// Interface allowing a <see cref="EntityHierarchyEditorViewModel"/> to safely access the camera of the game instance in which the editor is running.
    /// </summary>
    public interface IEditorGameEntityCameraViewModelService : IEditorGameCameraViewModelService
    {
        /// <summary>
        /// Gets instance of a <see cref="EditorCameraViewModel"/> class used by this camera view model service.
        /// </summary>
        [NotNull] EditorCameraViewModel Camera { get; }

        /// <summary>
        /// Centers the editor camera on the given entity.
        /// </summary>
        /// <param name="entity">The entity on which to center the camera.</param>
        /// <param name="meshIndex">The index of the mesh to center on, if relevant. Otherwise, -1 should be passed.</param>
        void CenterOnEntity(EntityViewModel entity, int meshIndex = -1);
    }
}
