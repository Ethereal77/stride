// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// A reference to a variable.
    /// </summary>
    public partial class TypeReferenceExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReferenceExpression"/> class.
        /// </summary>
        public TypeReferenceExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeReferenceExpression"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TypeReferenceExpression(TypeBase type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TypeBase Type { get; set; }

        /// <summary>
        /// Gets or sets the declaration.
        /// </summary>
        /// <value>
        /// The declaration.
        /// </value>
        public IDeclaration Declaration { get; set; }

        /// <inheritdoc/>
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.Add(Type);
            return ChildrenList;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
