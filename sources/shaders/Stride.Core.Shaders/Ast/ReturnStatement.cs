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
    /// A return statement.
    /// </summary>
    public partial class ReturnStatement : Statement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnStatement"/> class.
        /// </summary>
        public ReturnStatement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnStatement"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        #region Public Properties

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>
        ///   The value.
        /// </value>
        /// <remarks>
        ///   If this value is null, return without any expression.
        /// </remarks>
        public Expression Value { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.Add(Value);
            return ChildrenList;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("return{0};", Value != null ? " " + Value : string.Empty);
        }

        #endregion
    }
}
