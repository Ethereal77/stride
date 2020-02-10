// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
