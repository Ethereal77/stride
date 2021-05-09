// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// See the LICENSE.md file in the project root for full license information.

using System;

namespace NShader.Lexer
{
    public interface IShaderTokenProvider
    {
        ShaderToken GetTokenFromSemantics(string text);
        ShaderToken GetTokenFromIdentifier(string text);
    }
}
