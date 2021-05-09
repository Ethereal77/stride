// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.Package;

using NShader.Lexer;

namespace NShader
{
    public class NShaderScanner : IScanner
    {
        private Scanner lex;

        public NShaderScanner(IShaderTokenProvider tokenProvider)
        {
            lex = new Scanner();
            lex.ShaderTokenProvider = tokenProvider;
        }

        public void SetSource(string source, int offset)
        {
            lex.SetSource(source, offset);
        }

        public Scanner Lexer
        {
            get
            {
                return lex;
            }
        }

        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {

            int start, end;
            ShaderToken token = (ShaderToken)lex.GetNext(ref state, out start, out end);

            // !EOL and !EOF
            if (token != ShaderToken.EOF)
            {
                tokenInfo.StartIndex = start;
                tokenInfo.EndIndex = end;

                switch (token)
                {
                    case ShaderToken.KEYWORD:
                    case ShaderToken.TYPE:
                    case ShaderToken.KEYWORD_FX:
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Keyword;
                        break;
                    case ShaderToken.COMMENT:
                        tokenInfo.Color = TokenColor.Comment;
                        tokenInfo.Type = TokenType.Comment;
                        break;
                    case ShaderToken.COMMENT_LINE:
                        tokenInfo.Color = TokenColor.Comment;
                        tokenInfo.Type = TokenType.LineComment;
                        break;
                    case ShaderToken.NUMBER:
                    case ShaderToken.FLOAT:
                        tokenInfo.Color = TokenColor.Number;
                        tokenInfo.Type = TokenType.Literal;
                        break;
                    case ShaderToken.STRING_LITERAL:
                        tokenInfo.Color = TokenColor.String;
                        tokenInfo.Type = TokenType.Literal;
                        break;
                    case ShaderToken.INTRINSIC:
                        // hugly. TODO generate a NShaderTokenColor to keep tracks of 6-7-8 TokenColors
                        tokenInfo.Color = (TokenColor)6;
                        tokenInfo.Type = TokenType.Identifier;
                        break;
                    case ShaderToken.KEYWORD_SPECIAL:
                        tokenInfo.Color = (TokenColor)7;
                        tokenInfo.Type = TokenType.Identifier;
                        break;
                    case ShaderToken.PREPROCESSOR:
                        tokenInfo.Color = (TokenColor)8;
                        tokenInfo.Type = TokenType.Keyword;
                        break;
                    default:
                        tokenInfo.Color = TokenColor.Text;
                        tokenInfo.Type = TokenType.Text;
                        break;
                }
                return true;
            }
            return false;
        }
    }
}
