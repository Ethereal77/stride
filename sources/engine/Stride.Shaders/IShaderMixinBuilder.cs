// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Shaders
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
