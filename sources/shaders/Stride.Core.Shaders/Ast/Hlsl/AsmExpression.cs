// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Shaders.Ast.Hlsl
{
    /// <summary>
    /// A raw asm expression.
    /// </summary>
    public partial class AsmExpression : Expression
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the asm expression in raw text form.
        /// </summary>
        /// <value>
        ///   The asm expression in raw text form.
        /// </value>
        public string Text { get; set; }

        #endregion

        public override string ToString()
        {
            return "asm { ... }";
        }
    }
}
