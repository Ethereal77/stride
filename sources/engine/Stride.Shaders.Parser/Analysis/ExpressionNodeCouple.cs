// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Shaders.Ast;

namespace Stride.Shaders.Parser.Analysis
{
    [DataContract]
    internal class ExpressionNodeCouple
    {
        public Expression Expression;
        public Node Node;

        public ExpressionNodeCouple() : this(null, null) {}

        public ExpressionNodeCouple(Expression expression, Node node)
        {
            Expression = expression;
            Node = node;
        }
    }
}
