// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Serialization.Contents;
using Stride.Editor.EditorGame.ViewModels;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor.Services
{
    /// <summary>
    /// A service that provides access to debug information of an editor game.
    /// </summary>
    public interface IEditorGameDebugViewModelService : IEditorGameViewModelService
    {
        /// <summary>
        /// Gets the stats of the game asset manager.
        /// </summary>
        ContentManagerStats ContentManagerStats { get; }
    }
}
