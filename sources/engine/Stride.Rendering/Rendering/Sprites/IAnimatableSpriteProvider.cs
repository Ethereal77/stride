// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;

namespace Xenko.Engine
{
    /// <summary>
    /// The base interface for all classes providing animated sprites.
    /// </summary>
    [InlineProperty]
    public interface IAnimatableSpriteProvider : ISpriteProvider
    {
        /// <summary>
        /// Gets or sets the current frame of the animation.
        /// </summary>
        int CurrentFrame { get; set; }
    }
}
