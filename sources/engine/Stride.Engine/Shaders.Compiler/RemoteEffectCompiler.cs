// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.IO;
using Stride.Core.Storage;

namespace Stride.Shaders.Compiler
{
    /// <summary>
    ///   Represents a effect compiler that compiles the effects remotely on the developer's host PC.
    /// </summary>
    internal class RemoteEffectCompiler : EffectCompilerBase
    {
        private readonly DatabaseFileProvider database;
        private readonly RemoteEffectCompilerClient remoteEffectCompilerClient;

        /// <inheritdoc/>
        public override IVirtualFileProvider FileProvider { get; set; }

        public RemoteEffectCompiler(IVirtualFileProvider fileProvider, DatabaseFileProvider database, RemoteEffectCompilerClient remoteEffectCompilerClient)
        {
            FileProvider = fileProvider;
            this.database = database;
            this.remoteEffectCompilerClient = remoteEffectCompilerClient;
        }

        protected override void Destroy()
        {
            remoteEffectCompilerClient.Dispose();

            base.Destroy();
        }

        /// <inheritdoc/>
        public override ObjectId GetShaderSourceHash(string type)
        {
            var url = GetStoragePathFromShaderType(type);
            database.ContentIndexMap.TryGetValue(url, out var shaderSourceId);
            return shaderSourceId;
        }

        /// <inheritdoc/>
        public override TaskOrResult<EffectBytecodeCompilerResult> Compile(ShaderMixinSource mixinTree, EffectCompilerParameters effectParameters, CompilerParameters compilerParameters = null)
        {
            return CompileAsync(mixinTree, effectParameters);
        }

        private async Task<EffectBytecodeCompilerResult> CompileAsync(ShaderMixinSource mixinTree, EffectCompilerParameters effectParameters)
        {
            return await remoteEffectCompilerClient.Compile(mixinTree, effectParameters);
        }
    }
}
