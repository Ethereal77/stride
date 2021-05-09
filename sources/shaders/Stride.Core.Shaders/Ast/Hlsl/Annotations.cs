// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Shaders.Visitor;

namespace Stride.Core.Shaders.Ast.Hlsl
{
    /// <summary>
    /// An Annotations.
    /// </summary>
    public partial class Annotations : PostAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Annotations"/> class.
        /// </summary>
        public Annotations()
        {
            Variables = new List<Variable>();
        }

        /// <summary>
        /// Gets or sets the variable.
        /// </summary>
        /// <value>
        /// The variable.
        /// </value>
        public List<Variable> Variables { get; set; }
    }
}
