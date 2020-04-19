// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Ast;

namespace Xenko.Shaders.Parser.Analysis
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
