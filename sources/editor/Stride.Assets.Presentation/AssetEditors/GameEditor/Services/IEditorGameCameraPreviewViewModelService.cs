// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Editor.EditorGame.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor.Services
{
    /// <summary>
    /// Interface allowing a <see cref="ViewModels.GameEditorViewModel"/> to safely access a camera preview.
    /// </summary>
    public interface IEditorGameCameraPreviewViewModelService : IEditorGameViewModelService
    {
        bool IsActive { get; set; }
    }
}
