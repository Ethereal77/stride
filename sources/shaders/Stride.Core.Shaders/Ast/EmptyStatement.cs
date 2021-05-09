// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// A Empty of statement.
    /// </summary>
    public partial class EmptyStatement : Statement
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "EmptyStatement" /> class.
        /// </summary>
        public EmptyStatement()
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Empty;
        }
        #endregion
    }
}
