// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace NShader.Lexer
{
    public enum ShaderToken
    {
        EOF,
        UNDEFINED,
        PREPROCESSOR,
        KEYWORD,
        KEYWORD_FX,
        KEYWORD_SPECIAL,
        TYPE,
        IDENTIFIER,
        INTRINSIC,
        COMMENT_LINE,
        COMMENT,
        NUMBER,
        FLOAT,
        STRING_LITERAL,
        OPERATOR,
        DELIMITER,
        LEFT_BRACKET,
        RIGHT_BRACKET,
        LEFT_PARENTHESIS,
        RIGHT_PARENTHESIS,
        LEFT_SQUARE_BRACKET,
        RIGHT_SQUARE_BRACKET
    }
}
