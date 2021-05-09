// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Ast.Stride;
using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Visitor;

namespace Stride.Shaders.Parser.Mixins
{
    internal class StrideTagCleaner : ShaderWalker
    {
        public StrideTagCleaner()
            : base(false, false)
        {
        }

        public void Run(ShaderClassType shader)
        {
            Visit(shader);
        }

        public override void DefaultVisit(Node node)
        {
            // Keeping it for ShaderLinker (removed by StrideShaderCleaner)
            //node.RemoveTag(StrideTags.ConstantBuffer);
            node.RemoveTag(StrideTags.ShaderScope);
            node.RemoveTag(StrideTags.StaticRef);
            node.RemoveTag(StrideTags.ExternRef);
            node.RemoveTag(StrideTags.StageInitRef);
            node.RemoveTag(StrideTags.CurrentShader);
            node.RemoveTag(StrideTags.VirtualTableReference);
            node.RemoveTag(StrideTags.BaseDeclarationMixin);
            node.RemoveTag(StrideTags.ShaderScope);
            base.DefaultVisit(node);
        }
    }
}
