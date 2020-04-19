// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Serialization.Contents;
using Xenko.Editor.EditorGame.ViewModels;

namespace Xenko.Assets.Presentation.AssetEditors.GameEditor.Services
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
