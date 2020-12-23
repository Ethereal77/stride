// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.IO;
using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.BuildEngine;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;
using Stride.Shaders;
using Stride.Shaders.Compiler;
using Stride.Rendering;

namespace Stride.Assets.Effect
{
    /// <summary>
    ///   A <see cref="Command"/> that compiles a single permutation of an effect (SDFX or SDSL).
    /// </summary>
    internal sealed class EffectCompileCommand : IndexFileCommand
    {
        private static readonly PropertyKey<EffectCompilerBase> CompilerKey = new PropertyKey<EffectCompilerBase>("CompilerKey", typeof(EffectCompileCommand));

        private readonly AssetCompilerContext context;
        private readonly UDirectory baseUrl;
        private string effectName;
        private CompilerParameters compilerParameters;
        private readonly Package package;
        private static Dictionary<string, int> PermutationCount = new Dictionary<string, int>();

        public EffectCompileCommand(AssetCompilerContext context, UDirectory baseUrl, string effectName, CompilerParameters compilerParameters, Package package)
        {
            this.context = context;
            this.baseUrl = baseUrl;
            this.effectName = effectName;
            this.compilerParameters = compilerParameters;
            this.package = package;
        }

        public override string Title => $"EffectCompile [{effectName}]";

        protected override void ComputeParameterHash(BinarySerializationWriter writer)
        {
            base.ComputeParameterHash(writer);

            uint effectbyteCodeMagicNumber = EffectBytecode.MagicHeader;

            writer.Serialize(ref effectbyteCodeMagicNumber, ArchiveMode.Serialize);
            writer.Serialize(ref effectName, ArchiveMode.Serialize);
            writer.Serialize(ref compilerParameters, ArchiveMode.Serialize);
            writer.Serialize(ref compilerParameters.EffectParameters, ArchiveMode.Serialize);
        }

        protected override void ComputeAssemblyHash(BinarySerializationWriter writer)
        {
            writer.Write(DataSerializer.BinaryFormatVersion);
            writer.Write(EffectBytecode.MagicHeader);
        }

        protected override Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
        {
            var compiler = GetOrCreateEffectCompiler(context);

            // Get main effect name (before the first dot)
            var isSdfx = ShaderMixinManager.Contains(effectName);
            var source = isSdfx ?
                new ShaderMixinGeneratorSource(effectName) :
                (ShaderSource) new ShaderClassSource(effectName);

            int permutationCount;
            lock (PermutationCount)
            {
                PermutationCount.TryGetValue(effectName, out permutationCount);
                permutationCount++;
                PermutationCount[effectName] = permutationCount;
            }
            commandContext.Logger.Verbose($"Trying permutation #{permutationCount} for effect [{effectName}]: \n{compilerParameters.ToStringPermutationsDetailed()}");

            var compilerResults = compiler.Compile(source, compilerParameters);

            // Copy logs and if there are errors, exit directly
            compilerResults.CopyTo(commandContext.Logger);
            if (compilerResults.HasErrors)
            {
                return Task.FromResult(ResultStatus.Failed);
            }

            // Wait for result an check compilation status
            var completedTask = compilerResults.Bytecode.WaitForResult();
            completedTask.CompilationLog.CopyTo(commandContext.Logger);
            if (completedTask.CompilationLog.HasErrors)
            {
                return Task.FromResult(ResultStatus.Failed);
            }

            // Register all dependencies
            var allSources = new HashSet<string>(
                completedTask.Bytecode.HashSources.Select(keyPair => keyPair.Key));
            foreach (var className in allSources)
            {
                commandContext.RegisterInputDependency(new ObjectUrl(UrlType.Content, EffectCompilerBase.GetStoragePathFromShaderType(className)));
            }

            // Generate sourcecode if configured
            if (compilerParameters.ContainsKey(EffectSourceCodeKeys.Enable))
            {
                var outputDirectory = UPath.Combine(package.RootDirectory, baseUrl);

                var fieldName = compilerParameters.Get(EffectSourceCodeKeys.FieldName);
                if (fieldName.StartsWith("binary"))
                {
                    fieldName = fieldName.Substring("binary".Length);
                    if (char.IsUpper(fieldName[0]))
                    {
                        fieldName = char.ToLower(fieldName[0]) + fieldName.Substring(1);
                    }
                }

                var outputClassFile = effectName + "." + fieldName + "." + compilerParameters.EffectParameters.Platform + "." + compilerParameters.EffectParameters.Profile + ".cs";
                var fullOutputClassFile = Path.Combine(outputDirectory.ToWindowsPath(), outputClassFile);

                commandContext.Logger.Verbose($"Writing shader bytecode to .cs source [{fullOutputClassFile}].");
                using (var stream = new FileStream(fullOutputClassFile, FileMode.Create, FileAccess.Write, FileShare.Write))
                    EffectByteCodeToSourceCodeWriter.Write(effectName, compilerParameters, compilerResults.Bytecode.WaitForResult().Bytecode, new StreamWriter(stream, System.Text.Encoding.UTF8));
            }

            return Task.FromResult(ResultStatus.Successful);
        }

        public override string ToString() => Title;

        private static EffectCompilerBase GetOrCreateEffectCompiler(AssetCompilerContext context)
        {
            lock (context)
            {
                var compiler = context.Properties.Get(CompilerKey);
                if (compiler is null)
                {
                    // Create compiler
                    var effectCompiler = new EffectCompiler(MicrothreadLocalDatabases.DatabaseFileProvider);
                    effectCompiler.SourceDirectories.Add(EffectCompilerBase.DefaultSourceShaderFolder);
                    compiler = new EffectCompilerCache(effectCompiler, MicrothreadLocalDatabases.DatabaseFileProvider) { CurrentCache = EffectBytecodeCacheLoadSource.StartupCache };
                    context.Properties.Set(CompilerKey, compiler);

                    var shaderLocations = context.Properties.Get(EffectShaderAssetCompiler.ShaderLocationsKey);

                    // Temp copy URL to absolute file path to inform the compiler the absolute file location
                    // of all SDSL files.
                    if (shaderLocations != null)
                    {
                        foreach (var shaderLocation in shaderLocations)
                        {
                            effectCompiler.UrlToFilePath[shaderLocation.Key] = shaderLocation.Value;
                        }
                    }
                }

                return compiler;
            }
        }

        public static BuildStep FromRequest(AssetCompilerContext context, Package package, UDirectory urlRoot, EffectCompileRequest effectCompileRequest)
        {
            var compilerParameters = new CompilerParameters(effectCompileRequest.UsedParameters);

            compilerParameters.EffectParameters.Platform = context.GetGraphicsPlatform(package);
            compilerParameters.EffectParameters.Profile = context.GetGameSettingsAsset().GetOrCreate<RenderingSettings>(context.Platform).DefaultGraphicsProfile;
            compilerParameters.EffectParameters.ApplyCompilationMode(context.GetCompilationMode());

            return new CommandBuildStep(new EffectCompileCommand(context, urlRoot, effectCompileRequest.EffectName, compilerParameters, package));
        }
    }
}
