// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Ast;

namespace Xenko.Core.Shaders.Ast.Xenko
{
    /// <summary>
    /// A structure.
    /// </summary>
    public partial class VarType : TypeBase, IDeclaration, IScopeContainer
    {
        #region Constructors and Destructors
        /// <summary>
        ///   Initializes a new instance of the <see cref = "StructType" /> class.
        /// </summary>
        public VarType() : base("var")
        {
        }

        #endregion
    }
}
