// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Annotations;
using Stride.Engine;

namespace Stride.Editor.EditorGame.Game
{
    /// <summary>
    ///   Base interface for services that handle specific features of a <see cref="Stride.Engine.Game"/> instantiated for an asset editor.
    /// </summary>
    public interface IEditorGameService : IAsyncDisposable
    {
        /// <summary>
        ///   Gets whether this service has been initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///   Gets whether this service is currently active.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        ///   Gets the type of services that are required for this service to work.
        /// </summary>
        [ItemNotNull, NotNull]
        IEnumerable<Type> Dependencies { get; }

        /// <summary>
        ///   Initializes this service, allowing it to register scripts and modify the graphics compositor.
        /// </summary>
        /// <param name="game">The game for which this service is initialized.</param>
        /// <returns>Value indicating whether the service was initialized succesfully.</returns>
        /// <remarks>This method is invoked after the game is fully initialized.</remarks>
        Task<bool> InitializeService([NotNull] EditorServiceGame game);

        /// <summary>
        ///   Registers the given scene to this service, as the scene containing the objects being edited.
        /// </summary>
        /// <param name="scene">The scene to register.</param>
        void RegisterScene([NotNull] Scene scene);

        /// <summary>
        ///   Called when the game graphics compositor is updated.
        /// </summary>
        /// <param name="game"></param>
        void UpdateGraphicsCompositor([NotNull] EditorServiceGame game);
    }
}
