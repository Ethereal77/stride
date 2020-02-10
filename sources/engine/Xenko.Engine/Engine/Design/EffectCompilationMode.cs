// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Rendering;

namespace Xenko.Engine.Design
{
    /// <summary>
    /// Defines how <see cref="EffectCompilerFactory.CreateEffectCompiler"/> tries to create compiler.
    /// </summary>
    [Flags]
    public enum EffectCompilationMode
    {
        /// <summary>
        /// Effects can't be compiled. <see cref="Shaders.Compiler.NullEffectCompiler"/> will be used.
        /// </summary>
        None = 0,

        /// <summary>
        /// Effects can only be compiled in process (if possible). <see cref="Shaders.Compiler.EffectCompiler"/> will be used.
        /// </summary>
        Local = 1,

        /// <summary>
        /// Effects can only be compiled over network. <see cref="Shaders.Compiler.RemoteEffectCompiler"/> will be used.
        /// </summary>
        Remote = 2,

        /// <summary>
        /// Effects can be compiled either in process (if possible) or over network otherwise.
        /// </summary>
        LocalOrRemote = Local | Remote,
    }
}
