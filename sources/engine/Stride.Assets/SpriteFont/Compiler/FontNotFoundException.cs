// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Assets.SpriteFont.Compiler
{
    internal class FontNotFoundException : Exception
    {
        public FontNotFoundException(string fontName) : base(string.Format("Font with name [{0}] not found on this machine", fontName))
        {
            FontName = fontName;
        }

        public string FontName { get; private set; }
    }
}
