// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core.Diagnostics;
using Xenko.Rendering;

namespace Xenko.Shaders.Compiler
{
    /// <summary>
    /// Result of a compilation.
    /// </summary>
    public class CompilerResults : LoggerResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerResult" /> class.
        /// </summary>
        public CompilerResults() : base(null)
        {
        }

        /// <summary>
        /// Gets or sets the main bytecode.
        /// </summary>
        /// <value>
        /// The main bytecode.
        /// </value>
        public TaskOrResult<EffectBytecodeCompilerResult> Bytecode { get; set; }

        /// <summary>
        /// Parameters used to create this shader.
        /// </summary>
        /// <value>The ParameterCollection.</value>
        public CompilerParameters SourceParameters { get; set; }
    }
}
