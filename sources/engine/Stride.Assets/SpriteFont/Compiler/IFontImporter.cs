// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace Stride.Assets.SpriteFont.Compiler
{
    // Importer interface allows the conversion tool to support multiple source font formats.
    internal interface IFontImporter
    {
        /// <summary>
        /// Import Glyph and Bitmap of a sprite font asset.
        /// </summary>
        /// <param name="options">The sprite font asset to import</param>
        /// <param name="characters">The character set to import</param>
        void Import(SpriteFontAsset options, List<char> characters);

        IEnumerable<Glyph> Glyphs { get; }

        float LineSpacing { get; }
        float BaseLine { get; }
    }
}
