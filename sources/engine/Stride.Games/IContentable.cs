// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Games
{
    /// <summary>
    /// An interface to load and unload asset.
    /// </summary>
    public interface IContentable
    {
        /// <summary>
        /// Loads the assets.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.
        /// </summary>
        void UnloadContent();
    }
}
