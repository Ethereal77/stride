// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace NShader.Lexer
{
    public interface IShaderTokenProvider
    {
        ShaderToken GetTokenFromSemantics(string text);
        ShaderToken GetTokenFromIdentifier(string text);
    }
}
