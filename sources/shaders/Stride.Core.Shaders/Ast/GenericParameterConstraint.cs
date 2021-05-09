// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Shaders.Ast;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// A Generic parameter for a method that provides a constraint resolver.
    /// </summary>
    [DataContract]
    public class GenericParameterConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public GenericParameterConstraint(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="constraint">The constraint.</param>
        public GenericParameterConstraint(string name, Func<TypeBase, bool> constraint)
        {
            Name = name;
            Constraint = constraint;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the constraint match function.
        /// </summary>
        /// <value>
        /// The constraint match function.
        /// </value>
        public Func<TypeBase, bool> Constraint { get; set; }
    }
}
