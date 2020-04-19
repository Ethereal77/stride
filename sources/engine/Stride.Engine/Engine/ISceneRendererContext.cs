// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Core.Serialization.Contents;
using Xenko.Games;
using Xenko.Graphics;

namespace Xenko.Engine
{
    public interface ISceneRendererContext
    {
        /// <summary>
        /// The service registry.
        /// </summary>
        ServiceRegistry Services { get; }

        /// <summary>
        /// The list of game systems.
        /// </summary>
        GameSystemCollection GameSystems { get; }

        /// <summary>
        /// The current scene system.
        /// </summary>
        SceneSystem SceneSystem { get; }

        /// <summary>
        /// The graphics device.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// The graphics context used during draw.
        /// </summary>
        GraphicsContext GraphicsContext { get; }

        /// <summary>
        ///  The content manager to load content.
        /// </summary>
        ContentManager Content { get; }

        GameTime DrawTime { get; }
    }
}
