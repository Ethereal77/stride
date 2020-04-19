// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;
using Xenko.Graphics;

namespace Xenko.Engine
{
    /// <summary>
    /// The base interface for all classes providing sprites.
    /// </summary>
    [InlineProperty]
    public interface ISpriteProvider
    {
        /// <summary>
        /// Gets the number of sprites available in the provider.
        /// </summary>
        int SpritesCount { get; }

        /// <summary>
        /// Get a sprite from the provider.
        /// </summary>
        /// <returns></returns>
        Sprite GetSprite();
    }
}
