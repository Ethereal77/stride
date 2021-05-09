// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Editor.EditorGame.Game;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor.Game
{
    /// <summary>
    ///   Defines the interface of a <see cref="IEditorGameService"/> that can control the mouse.
    /// </summary>
    public interface IEditorGameMouseService : IEditorGameService
    {
        /// <summary>
        ///   Gets a value indicating whether this instance is currently controlling the mouse.
        /// </summary>
        bool IsControllingMouse { get; }

        /// <summary>
        ///   Gets a value indicating whether the mouse is available to be controlled.
        /// </summary>
        bool IsMouseAvailable { get; }
    }
}
