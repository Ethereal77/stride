// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Diagnostics;
using Stride.Rendering;

namespace Stride.Shaders.Compiler
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
