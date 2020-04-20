// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// Licensed under Microsoft Public License (Ms-PL)
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Stride.Graphics;
using Stride.Graphics.Font;

namespace Stride.Assets.SpriteFont.Compiler
{
    /// <summary>
    /// Main class used to compile a Font file XML file.
    /// </summary>
    public class SignedDistanceFieldFontCompiler
    {
        /// <summary>
        /// Compiles the specified font description into a <see cref="SignedDistanceFieldSpriteFont" /> object.
        /// </summary>
        /// <param name="fontFactory">The font factory used to create the fonts</param>
        /// <param name="fontAsset">The font description.</param>
        /// <returns>A SpriteFontData object.</returns>
        public static Graphics.SpriteFont Compile(IFontFactory fontFactory, SpriteFontAsset fontAsset)
        {
            var fontTypeSDF = fontAsset.FontType as SignedDistanceFieldSpriteFontType;
            if (fontTypeSDF == null)
                throw new ArgumentException("Tried to compile a dynamic sprite font with compiler for signed distance field fonts");

            float lineSpacing;
            float baseLine;

            var glyphs = ImportFont(fontAsset, out lineSpacing, out baseLine);

            Bitmap bitmap = GlyphPacker.ArrangeGlyphs(glyphs);

            return SignedDistanceFieldFontWriter.CreateSpriteFontData(fontFactory, fontAsset, glyphs, lineSpacing, baseLine, bitmap);
        }

        static Glyph[] ImportFont(SpriteFontAsset options, out float lineSpacing, out float baseLine)
        {
            // Which importer knows how to read this source font?
            IFontImporter importer;

            var sourceExtension = (Path.GetExtension(options.FontSource.GetFontPath()) ?? "").ToLowerInvariant();
            var bitmapFileExtensions = new List<string> { ".bmp", ".png", ".gif" };
            var importFromBitmap = bitmapFileExtensions.Contains(sourceExtension);
            if (importFromBitmap)
            {
                throw new Exception("SDF Font from image is not supported!");
            }

            importer = new SignedDistanceFieldFontImporter();

            // create the list of character to import
            var characters = GetCharactersToImport(options);

            // Import the source font data.
            importer.Import(options, characters);

            lineSpacing = importer.LineSpacing;
            baseLine = importer.BaseLine;

            // Get all glyphs
            var glyphs = new List<Glyph>(importer.Glyphs);

            // Validate.
            if (glyphs.Count == 0)
            {
                throw new Exception("Font does not contain any glyphs.");
            }

            // Sort the glyphs
            glyphs.Sort((left, right) => left.Character.CompareTo(right.Character));

            // Check that the default character is part of the glyphs
            if (!DefaultCharacterExists(options.DefaultCharacter, glyphs))
            {
                throw new InvalidOperationException("The specified DefaultCharacter is not part of this font.");
            }

            return glyphs.ToArray();
        }

        private static bool DefaultCharacterExists(char defaultCharacter, List<Glyph> glyphs)
        {
            if (defaultCharacter == 0)
                return true;

            foreach (var glyph in glyphs)
            {
                if (glyph.Character == defaultCharacter)
                    return true;
            }

            return false;
        }

        public static List<char> GetCharactersToImport(SpriteFontAsset asset)
        {
            var characters = new List<char>();

            var fontTypeSDF = asset.FontType as SignedDistanceFieldSpriteFontType;
            if (fontTypeSDF == null)
                throw new ArgumentException("Tried to compile a dynamic sprite font with compiler for signed distance field fonts");
            
            // extract the list from the provided file if it exits
            if (File.Exists(fontTypeSDF.CharacterSet))
            {
                string text;
                using (var streamReader = new StreamReader(fontTypeSDF.CharacterSet, Encoding.UTF8))
                    text = streamReader.ReadToEnd();
                characters.AddRange(text);
            }

            // add character coming from character ranges
            characters.AddRange(CharacterRegion.Flatten(fontTypeSDF.CharacterRegions));

            // remove duplicated characters
            characters = characters.Distinct().ToList();

            return characters;
        }
    }
}
