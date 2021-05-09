// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Ast;
using Stride.Core.Shaders.Visitor;

namespace Stride.Shaders.Parser.Mixins
{
    internal class StrideReplaceExtern : ShaderRewriter
    {
        #region Private members

        /// <summary>
        /// The variable to replace
        /// </summary>
        private Variable VariableToReplace = null;

        /// <summary>
        /// the expression that will replace the variable
        /// </summary>
        private IndexerExpression IndexerReplacement = null;

        #endregion

        #region Constructor

        public StrideReplaceExtern(Variable variable, IndexerExpression expression)
            : base(false, true)
        {
            VariableToReplace = variable;
            IndexerReplacement = expression;
        }

        public void Run(Node initialNode)
        {
            VisitDynamic(initialNode);
        }

        #endregion

        public override Node Visit(MemberReferenceExpression expression)
        {
            base.Visit(expression);
            if (expression.Member.Text == VariableToReplace.Name.Text)
                return new IndexerExpression(new MemberReferenceExpression(expression.Target, (IndexerReplacement.Target as VariableReferenceExpression).Name.Text), IndexerReplacement.Index);

            return expression;
        }

        public override Node Visit(VariableReferenceExpression expression)
        {
            base.Visit(expression);
            if (expression.Name.Text == VariableToReplace.Name.Text)
                return IndexerReplacement;

            return expression;
        }
    }
}
