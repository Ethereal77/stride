// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Shaders.Ast;

namespace Stride.Core.Shaders.Ast.Stride
{
    /// <summary>
    /// A using statement.
    /// </summary>
    public partial class UsingStatement : Statement
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "UsingStatement" /> class.
        /// </summary>
        public UsingStatement()
        {
        }

        public Identifier Name;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.Add(Name);
            return ChildrenList;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("using {0}",  Name);
        }

        #endregion
    }
}
