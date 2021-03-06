// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;

namespace Stride.Graphics.Font
{
    /// <summary>
    /// Description of a glyph (a single character)
    /// </summary>
    [DataContract]
    public class Glyph
    {
        /// <summary>
        /// Unicode codepoint.
        /// </summary>
        public int Character;

        /// <summary>
        /// Glyph image data (may only use a portion of a larger bitmap).
        /// </summary>
        public Rectangle Subrect;

        /// <summary>
        /// Layout information.
        /// </summary>
        public Vector2 Offset;

        /// <summary>
        /// Advance X
        /// </summary>
        public float XAdvance;

        /// <summary>
        /// Index of the bitmap. 
        /// </summary>
        public int BitmapIndex;
    } 
}
