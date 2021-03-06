// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Mathematics;
using Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.ViewModels;
using Stride.Editor.EditorGame.ViewModels;
using Stride.Rendering;

namespace Stride.Assets.Presentation.AssetEditors.EntityHierarchyEditor.Services
{
    /// <summary>
    /// A service that renders navigation mesh overlays in the scene
    /// </summary>
    public interface IEditorGameNavigationViewModelService : IEditorGameViewModelService
    {
        /// <summary>
        /// Visibility of all the visuals
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Updates how many groups there are and in what colors to show them
        /// </summary>
        void UpdateGroups(IList<EditorNavigationGroupViewModel> groups);

        /// <summary>
        /// Updates group visibility, based on display settings
        /// </summary>
        void UpdateGroupVisibility(Guid groupId, bool isVisible);
    }
}
