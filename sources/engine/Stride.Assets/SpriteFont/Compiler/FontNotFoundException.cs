// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
