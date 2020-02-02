// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Xenko.Core.Shaders.Ast
{
    /// <summary>
    /// Expression used to initliaze an array {...expressions,}
    /// </summary>
    public partial class ArrayInitializerExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayInitializerExpression"/> class.
        /// </summary>
        public ArrayInitializerExpression()
        {
            Items = new List<Expression>();
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<Expression> Items { get; set; }

        /// <inheritdoc/>
        public override IEnumerable<Node> Childrens()
        {
            return Items;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(",", Items));
        }
    }
}
