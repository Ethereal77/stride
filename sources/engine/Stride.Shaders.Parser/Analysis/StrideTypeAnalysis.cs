// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Shaders.Ast.Stride;
using Stride.Core.Shaders.Analysis.Hlsl;
using Stride.Core.Shaders.Parser;

namespace Stride.Shaders.Parser.Analysis
{
    internal class StrideTypeAnalysis : HlslSemanticAnalysis
    {
        #region Contructor

        public StrideTypeAnalysis(ParsingResult result)
            : base(result)
        {
            SetupHlslAnalyzer();
        }

        #endregion

        public void Run(ShaderClassType shaderClassType)
        {
            Visit(shaderClassType);
        }
    }
}
