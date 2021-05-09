// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// A typeless reference.
    /// </summary>
    public partial class TypeName : TypeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeName"/> class.
        /// </summary>
        public TypeName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeName"/> class.
        /// </summary>
        /// <param name="typeBase">The type base.</param>
        public TypeName(TypeBase typeBase)
            : base(typeBase.Name)
        {
            TypeInference.TargetType = typeBase;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TypeName(Identifier name) : base(name)
        {
        }
    }
}
