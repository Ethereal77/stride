// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Shaders;

namespace Xenko.Rendering
{
    /// <summary>
    /// Defines the interface to provide an effect mixin for a <see cref="CameraRendererMode"/>.
    /// </summary>
    public interface IEffectMixinProvider
    {
        /// <summary>
        /// Generates the shader source used for rendering.
        /// </summary>
        /// <returns>ShaderSource.</returns>
        ShaderSource GenerateShaderSource();
    }

    [DataContract("DefaultEffectMixinProvider")]
    public class DefaultEffectMixinProvider : IEffectMixinProvider
    {
        private readonly ShaderSource shaderSource;

        public DefaultEffectMixinProvider(string name)
        {
            shaderSource = new ShaderClassSource(name);
        }

        public ShaderSource GenerateShaderSource()
        {
            return shaderSource;
        }
    }
}
