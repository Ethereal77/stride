// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Shaders.Ast;

namespace Xenko.Core.Shaders.Ast.Xenko
{
    /// <summary>
    /// A typeless reference.
    /// </summary>
    public partial class ShaderTypeName : TypeName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderTypeName"/> class.
        /// </summary>
        public ShaderTypeName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderTypeName"/> class.
        /// </summary>
        /// <param name="typeBase">The type base.</param>
        public ShaderTypeName(TypeBase typeBase)
            : base(typeBase.Name)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderTypeName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ShaderTypeName(Identifier name) : base(name)
        {
        }
    }
}
