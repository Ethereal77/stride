// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Ast;

namespace Stride.Shaders.Parser.Analysis
{
    public class MemberReferenceExpressionNodeCouple
    {
        public MemberReferenceExpression Member;
        public Node Node;

        public MemberReferenceExpressionNodeCouple() : this(null, null) { }

        public MemberReferenceExpressionNodeCouple(MemberReferenceExpression member, Node node)
        {
            Member = member;
            Node = node;
        }
    }
}
