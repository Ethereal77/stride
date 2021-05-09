// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// See the LICENSE.md file in the project root for full license information.

using NShader.Lexer;

namespace NShader
{
    public class HLSLShaderTokenProvider : IShaderTokenProvider
    {
        private static EnumMap<ShaderToken> map;

        static HLSLShaderTokenProvider()
        {
            map = new EnumMap<ShaderToken>();
            map.Load("HLSLKeywords.map");
        }

        public ShaderToken GetTokenFromSemantics(string text)
        {
            text = text.Replace(" ", "");
            ShaderToken token;
            if (!map.TryGetValue(text.ToUpperInvariant(), out token))
            {
                token = ShaderToken.IDENTIFIER;
            }
            return token;
        }

        public ShaderToken GetTokenFromIdentifier(string text)
        {
            ShaderToken token;
            if ( ! map.TryGetValue(text, out token ) )
            {
                token = ShaderToken.IDENTIFIER;
            }
            return token;
        }
    }
}
