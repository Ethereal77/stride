// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Diagnostics;
using Stride.Core.IO;
using Stride.Core.Storage;

namespace Stride.Shaders.Compiler
{
    /// <summary>
    /// Helper class that delegates actual compilation to another <see cref="IEffectCompiler"/>.
    /// </summary>
    public class EffectCompilerChain : EffectCompilerBase
    {
        private readonly EffectCompilerBase compiler;

        public EffectCompilerChain(EffectCompilerBase compiler)
        {
            if (compiler == null) throw new ArgumentNullException("compiler");
            this.compiler = compiler;
        }

        protected EffectCompilerBase Compiler
        {
            get { return compiler; }
        }

        public override IVirtualFileProvider FileProvider
        {
            get { return compiler.FileProvider; }
            set { compiler.FileProvider = value; }
        }

        protected override void Destroy()
        {
            compiler.Dispose();

            base.Destroy();
        }

        public override ObjectId GetShaderSourceHash(string type)
        {
            return compiler.GetShaderSourceHash(type);
        }

        public override void ResetCache(HashSet<string> modifiedShaders)
        {
            compiler.ResetCache(modifiedShaders);
        }

        public override TaskOrResult<EffectBytecodeCompilerResult> Compile(ShaderMixinSource mixinTree, EffectCompilerParameters effectParameters, CompilerParameters compilerParameters = null)
        {
            return compiler.Compile(mixinTree, effectParameters, compilerParameters);
        }
    }
}
