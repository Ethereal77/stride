// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System.Drawing;

namespace Xenko.Assets.SpriteFont.Compiler
{
    // Represents a single character within a font.
    internal class Glyph
    {
        // Constructor.
        public Glyph(char character, Bitmap bitmap, Rectangle? subrect = null)
        {
            this.Character = character;
            this.Bitmap = bitmap;
            this.Subrect = subrect.GetValueOrDefault(new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }


        // Unicode codepoint.
        public char Character;


        // Glyph image data (may only use a portion of a larger bitmap).
        public Bitmap Bitmap;
        public Rectangle Subrect;
        

        // Layout information.
        public float XOffset;
        public float YOffset;

        public float XAdvance;
    }
}
