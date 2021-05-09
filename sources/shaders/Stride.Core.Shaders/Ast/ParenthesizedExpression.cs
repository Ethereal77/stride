// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// An expression surrounded by parenthesis.
    /// </summary>
    public partial class ParenthesizedExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParenthesizedExpression"/> class.
        /// </summary>
        public ParenthesizedExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParenthesizedExpression"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public ParenthesizedExpression(params Expression[] content)
        {
            if (content != null)
            {
                if (content.Length == 1)
                    Content = content[0];
                else
                    Content = new ExpressionList(content);
            }
        }

        #region Public Properties

        /// <summary>
        ///   Gets or sets the expression.
        /// </summary>
        /// <value>
        ///   The expression.
        /// </value>
        public Expression Content { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.Add(Content);
            return ChildrenList;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("({0})", string.Join(",", Content));
        }

        #endregion
    }
}
