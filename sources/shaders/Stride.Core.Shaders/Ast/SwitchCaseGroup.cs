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
    /// A group of cases and default attached to their statements.
    /// </summary>
    public partial class SwitchCaseGroup : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchCaseGroup"/> class.
        /// </summary>
        public SwitchCaseGroup()
        {
            Cases = new List<CaseStatement>();
            Statements = new StatementList();
        }

        #region Public Properties

        /// <summary>
        ///   Gets or sets the cases.
        /// </summary>
        /// <value>
        ///   The cases.
        /// </value>
        public List<CaseStatement> Cases { get; set; }

        /// <summary>
        ///   Gets or sets the statements.
        /// </summary>
        /// <value>
        ///   The statements.
        /// </value>
        public StatementList Statements { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override IEnumerable<Node> Childrens()
        {
            ChildrenList.Clear();
            ChildrenList.AddRange(Cases);
            ChildrenList.Add(Statements);
            return ChildrenList;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("{0} ...", string.Join("\n", Cases));
        }

        #endregion
    }
}
