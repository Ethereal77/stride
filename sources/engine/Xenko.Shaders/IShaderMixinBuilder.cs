// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Shaders
{
    /// <summary>
    /// Interface to be implemented for dynamic mixin generation.
    /// </summary>
    public interface IShaderMixinBuilder
    {
        /// <summary>
        /// Generates a mixin.
        /// </summary>
        /// <param name="mixinTree">The mixin tree.</param>
        /// <param name="context">The context.</param>
        void Generate(ShaderMixinSource mixinTree, ShaderMixinContext context);
    }
}
