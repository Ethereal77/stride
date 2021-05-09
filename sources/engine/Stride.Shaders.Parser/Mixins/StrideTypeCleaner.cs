// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Visitor;

namespace Stride.Shaders.Parser.Mixins
{
    internal class StrideTypeCleaner : ShaderWalker
    {
        public StrideTypeCleaner()
            : base(false, false)
        {
        }

        public void Run(Shader shader)
        {
            Visit(shader);
        }

        public override void DefaultVisit(Node node)
        {
            if (node is Expression || node is TypeBase)
                VisitTypeInferencer((ITypeInferencer)node);

            base.DefaultVisit(node);
        }

        private void VisitTypeInferencer(ITypeInferencer expression)
        {
            expression.TypeInference.Declaration = null;
            expression.TypeInference.TargetType = null;
            expression.TypeInference.ExpectedType = null;
        }
    }
}
