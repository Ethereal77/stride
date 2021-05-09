// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Graphics
{
    /// <summary>
    /// A sprite frame represents a single frame a sprite animation.
    /// It contains the region of the texture representing the frame and the time it is supposed to be displayed.
    /// </summary>
    [DataContract]
    public class SpriteFrame
    {
        /// <summary>
        /// The rectangle specifying the region of the texture to use for that frame.
        /// </summary>
        public Rectangle TextureRegion;

        /// <summary>
        /// The bias to the frame center in pixels.
        /// </summary>
        public Vector2 CenterBias;

        /// <summary>
        /// Clone the current sprite frame instance.
        /// </summary>
        /// <returns>A new instance of the current sprite frame.</returns>
        public SpriteFrame Clone()
        {
            return (SpriteFrame)MemberwiseClone();
        }
    }
}
