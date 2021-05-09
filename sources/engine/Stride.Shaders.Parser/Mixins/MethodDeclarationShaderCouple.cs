// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Shaders.Ast.Stride;
using Stride.Core.Shaders.Ast;

namespace Stride.Shaders.Parser.Mixins
{
    [DataContract]
    internal class MethodDeclarationShaderCouple
    {
        public MethodDeclaration Method;
        public ShaderClassType Shader;

        public MethodDeclarationShaderCouple() : this(null, null){}

        public MethodDeclarationShaderCouple(MethodDeclaration method, ShaderClassType shader)
        {
            Method = method;
            Shader = shader;
        }
    }
}
