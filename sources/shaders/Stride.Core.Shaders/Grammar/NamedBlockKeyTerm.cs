// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using GoldParser;

using Stride.Irony.Parsing;

namespace Stride.Core.Shaders.Grammar
{
    internal class NamedBlockKeyTerm : DynamicKeyTerm
    {
        public NamedBlockKeyTerm(string text, string name)
            : base(text, name)
        {
        }

        public override void Match(Tokenizer toknizer, out Token token)
        {
            var parser = toknizer.GoldParser;

            var location = toknizer.Location;
            var nextSymbol = parser.ReadToken();

            while (nextSymbol.SymbolType == SymbolType.WhiteSpace || nextSymbol.Index == (int)TokenType.NewLine)
                nextSymbol = parser.ReadToken();

            if (nextSymbol.Index != (int)TokenType.LeftCurly)
            {
                token = new Token(Grammar.SyntaxError, location, parser.TokenText, "Expecting '{'");
                return;
            }

            var startPosition = parser.CharPosition;

            bool rightCurlyFound = false;
            int length = 0;
            for (int i = parser.CharPosition; i < parser.TextBuffer.Length; i++, length++)
            {
                if (parser.TextBuffer[i] == '}')
                {
                    rightCurlyFound = true;
                    break;
                }
            }

            if (!rightCurlyFound)
            {
                token = new Token(Grammar.SyntaxError, location, parser.TokenText, "Closing '}' not found");
                return;                
            }

            token = new Token(this, location, parser.TextBuffer.Substring(startPosition, length), null);

            // Move the parser
            parser.MoveBy(length+1);
        }
    }
}
