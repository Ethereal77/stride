// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xenko.Rendering;

namespace Xenko.Shaders.Compiler
{
    /// <summary>
    /// Main interface used to compile a shader.
    /// </summary>
    public interface IEffectCompiler : IDisposable
    {
        /// <summary>
        /// Compiles the specified shader source.
        /// </summary>
        /// <param name="shaderSource">The shader source.</param>
        /// <param name="compilerParameters">The compiler parameters.</param>
        /// <returns>Result of the compilation.</returns>
        CompilerResults Compile(ShaderSource shaderSource, CompilerParameters compilerParameters);
    }
}
