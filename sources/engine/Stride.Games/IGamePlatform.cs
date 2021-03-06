// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Games
{
    /// <summary>
    /// Interface for a game platform (OS, machine dependent).
    /// </summary>
    public interface IGamePlatform
    {
        /// <summary>
        /// Gets the default app directory.
        /// </summary>
        /// <value>The default app directory.</value>
        string DefaultAppDirectory { get; }

        /// <summary>
        /// Gets the main window.
        /// </summary>
        /// <value>The main window.</value>
        GameWindow MainWindow { get; }

        /// <summary>
        /// Creates the a new <see cref="GameWindow"/>. See remarks.
        /// </summary>
        /// <param name="gameContext">The window context. See remarks.</param>
        /// <returns>A new game window.</returns>
        /// <remarks>
        /// This is currently only supported on Windows. The window context supported on Windows is a subclass of System.Windows.Forms.Control (or null and a default GameForm will be created).
        /// </remarks>
        GameWindow CreateWindow(GameContext gameContext = null);
    }
}
