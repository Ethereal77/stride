// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Serialization.Contents;
using Stride.Games;
using Stride.Graphics;

namespace Stride.Engine
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
