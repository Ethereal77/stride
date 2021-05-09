// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Graphics;

namespace Stride.Engine
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
