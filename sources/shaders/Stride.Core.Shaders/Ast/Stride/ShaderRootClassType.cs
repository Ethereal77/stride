// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Shaders.Ast.Xenko
{
    /// <summary>
    /// Shader Class that supports adding mixin class to its base classes.
    /// </summary>
    public partial class ShaderRootClassType : ShaderClassType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderRootClassType"/> class.
        /// </summary>
        public ShaderRootClassType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderRootClassType"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ShaderRootClassType(string name)
            : base(name)
        {
        }
    }
}
