// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;
using Stride.Rendering;
using Stride.Graphics;

namespace Stride.Shaders.Compiler
{
    internal class ShaderBytecodeResult : LoggerResult
    {
        public ShaderBytecode Bytecode { get; set; }

        public string DisassembleText { get; set; }
    }

    internal interface IShaderCompiler
    {
        ShaderBytecodeResult Compile(string shaderSource, string entryPoint, ShaderStage stage, EffectCompilerParameters effectParameters, EffectReflection reflection, string sourceFilename = null);
    }
}
