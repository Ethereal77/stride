// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Core.IO;
using Stride.Core.ReferenceCounting;
using Stride.Games;
using Stride.Graphics;
using Stride.Shaders;
using Stride.Shaders.Compiler;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a system responsible for the loading, compilation and caching of effects.
    /// </summary>
    public class EffectSystem : GameSystemBase
    {
        private static readonly Logger Log = GlobalLogger.GetLogger("EffectSystem");

        private EffectCompilerParameters effectCompilerParameters = EffectCompilerParameters.Default;

        private EffectCompilerBase compiler;
        private readonly Dictionary<string, List<CompilerResults>> earlyCompilerCache = new Dictionary<string, List<CompilerResults>>();
        private readonly Dictionary<EffectBytecode, Effect> cachedEffects = new Dictionary<EffectBytecode, Effect>();
        private DirectoryWatcher directoryWatcher;

        private bool isInitialized;

        /// <summary>
        ///   Called each time a non-cached effect is requested.
        /// </summary>
        internal Action<EffectCompileRequest, CompilerResults> EffectUsed;

        private readonly HashSet<string> recentlyModifiedShaders = new HashSet<string>();

        public IEffectCompiler Compiler
        {
            get => compiler;
            set => compiler = (EffectCompilerBase) value;
        }

        /// <summary>
        ///   Gets the file provider to use for loading effects and shader sources.
        /// </summary>
        /// <value>The file provider.</value>
        public IVirtualFileProvider FileProvider => compiler.FileProvider;


        /// <summary>
        ///   Initializes a new instance of the <see cref="EffectSystem"/> class.
        /// </summary>
        /// <param name="services">The service registry.</param>
        public EffectSystem(IServiceRegistry services) : base(services) { }

        public override void Initialize()
        {
            base.Initialize();

            // Get graphics device service
            InitGraphicsDeviceService();

            Enabled = true;

            directoryWatcher = new DirectoryWatcher("*.sdsl");
            directoryWatcher.Modified += FileModifiedEvent;
            // TODO: SDFX too.

            isInitialized = true;
        }

        public void SetCompilationMode(CompilationMode compilationMode)
        {
            effectCompilerParameters.ApplyCompilationMode(compilationMode);
        }

        protected override void Destroy()
        {
            // Mark effect system as destroyed (so that async effect compilation is ignored)
            lock (cachedEffects)
            {
                // Clear effects
                foreach (var effect in cachedEffects)
                {
                    effect.Value.ReleaseInternal();
                }
                cachedEffects.Clear();

                // Mark as not initialized anymore
                isInitialized = false;
            }

            if (directoryWatcher != null)
            {
                directoryWatcher.Modified -= FileModifiedEvent;
                directoryWatcher.Dispose();
                directoryWatcher = null;
            }

            Compiler?.Dispose();
            Compiler = null;

            base.Destroy();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateEffects();
        }

        public bool IsValid(Effect effect)
        {
            lock (cachedEffects)
            {
                return cachedEffects.ContainsKey(effect.Bytecode);
            }
        }

        /// <summary>
        ///   Loads an effect.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="compilerParameters">The compiler parameters.</param>
        /// <returns>The new instance of effect loaded.</returns>
        /// <exception cref="InvalidOperationException">Could not compile shader. Need fallback.</exception>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="effectName"/> or <paramref name="compilerParameters"/> are a <c>null</c> reference.
        /// </exception>
        public TaskOrResult<Effect> LoadEffect(string effectName, CompilerParameters compilerParameters)
        {
            if (effectName is null)
                throw new ArgumentNullException(nameof(effectName));
            if (compilerParameters is null)
                throw new ArgumentNullException(nameof(compilerParameters));

            // Setup compilation parameters
            // GraphicsDevice might have been not valid until this point, which is why we compute platform and profile only at this point
            compilerParameters.EffectParameters.Platform = GraphicsDevice.Platform;
            compilerParameters.EffectParameters.Profile = GraphicsDevice.ShaderProfile ?? GraphicsDevice.Features.RequestedProfile;
            // Copy optimization/debug levels
            compilerParameters.EffectParameters.OptimizationLevel = effectCompilerParameters.OptimizationLevel;
            compilerParameters.EffectParameters.Debug = effectCompilerParameters.Debug;

            // Get the compiled result
            var compilerResult = GetCompilerResults(effectName, compilerParameters);
            CheckResult(compilerResult);

            // Only take the sub-effect
            var bytecode = compilerResult.Bytecode;

            if (bytecode.Task != null && !bytecode.Task.IsCompleted)
            {
                // Result was async, keep it async
                // NOTE: There was some hangs when doing ContinueWith() (it might switch from EffectPriorityScheduler to TaskScheduler.Default, maybe something doesn't work well in this case?)
                //       It seems that TaskContinuationOptions.ExecuteSynchronously is helping in this case (also it will force continuation to execute right away on the thread pool, which is probably better)
                //       Not sure if the probably totally disappeared (esp. if something does a ContinueWith() externally on that) -- might need further investigation.
                var result = bytecode.Task.ContinueWith(
                    x => CreateEffect(effectName, x.Result, compilerResult),
                    TaskContinuationOptions.ExecuteSynchronously);
                return result;
            }
            else
            {
                return CreateEffect(effectName, bytecode.WaitForResult(), compilerResult);
            }
        }

        // TODO: THIS IS JUST A WORKAROUND, REMOVE THIS

        private static void CheckResult(LoggerResult compilerResult)
        {
            // Check errors
            if (compilerResult.HasErrors)
                throw new InvalidOperationException("Could not compile shader. See error messages." + compilerResult.ToText());
        }

        private Effect CreateEffect(string effectName, EffectBytecodeCompilerResult effectBytecodeCompilerResult, CompilerResults compilerResult)
        {
            Effect effect;
            lock (cachedEffects)
            {
                if (!isInitialized)
                    throw new ObjectDisposedException(nameof(EffectSystem), "EffectSystem has been disposed. This Effect compilation has been cancelled.");

                if (effectBytecodeCompilerResult.CompilationLog.HasErrors)
                {
                    // Unregister result
                    // TODO: Should we keep it so that failure never change?
                    if (earlyCompilerCache.TryGetValue(effectName, out List<CompilerResults> effectCompilerResults))
                    {
                        effectCompilerResults.Remove(compilerResult);
                    }
                }

                CheckResult(effectBytecodeCompilerResult.CompilationLog);

                var bytecode = effectBytecodeCompilerResult.Bytecode;
                if (bytecode is null)
                    throw new InvalidOperationException("EffectCompiler returned no shader and no compilation error.");

                if (!cachedEffects.TryGetValue(bytecode, out effect))
                {
                    effect = new Effect(GraphicsDevice, bytecode) { Name = effectName };
                    cachedEffects.Add(bytecode, effect);

                    foreach (var type in bytecode.HashSources.Keys)
                    {
                        var storagePath = EffectCompilerBase.GetStoragePathFromShaderType(type);
                        if (!FileProvider.TryGetFileLocation(storagePath, out var filePath, out _, out _))
                        {
                            // TODO: the "/path" is hardcoded, used in ImportStreamCommand and ShaderSourceManager. Find a place to share this correctly.
                            var pathUrl = storagePath + "/path";
                            if (FileProvider.FileExists(pathUrl))
                            {
                                using (var pathStream = FileProvider.OpenStream(pathUrl, VirtualFileMode.Open, VirtualFileAccess.Read))
                                using (var reader = new StreamReader(pathStream))
                                {
                                    filePath = reader.ReadToEnd();
                                }
                            }
                        }
                        if (filePath != null)
                            directoryWatcher.Track(filePath);
                    }
                }
            }
            return effect;
        }

        private CompilerResults GetCompilerResults(string effectName, CompilerParameters compilerParameters)
        {
            // Compile shader
            var isSdfx = ShaderMixinManager.Contains(effectName);

            // Getting the effect from the used parameters only makes sense when the source files are the same
            // TODO: Improve this by updating earlyCompilerCache - cache can still be relevant

            CompilerResults compilerResult = null;

            if (isSdfx)
            {
                // Perform an early test only based on the parameters
                compilerResult = GetShaderFromParameters(effectName, compilerParameters);
            }

            if (compilerResult is null)
            {
                var source = isSdfx ?
                    new ShaderMixinGeneratorSource(effectName) :
                    (ShaderSource) new ShaderClassSource(effectName);

                compilerResult = compiler.Compile(source, compilerParameters);

                EffectUsed?.Invoke(new EffectCompileRequest(effectName, new CompilerParameters(compilerParameters)), compilerResult);

                if (!compilerResult.HasErrors && isSdfx)
                {
                    lock (earlyCompilerCache)
                    {
                        if (!earlyCompilerCache.TryGetValue(effectName, out List<CompilerResults> effectCompilerResults))
                        {
                            effectCompilerResults = new List<CompilerResults>();
                            earlyCompilerCache.Add(effectName, effectCompilerResults);
                        }

                        // Register bytecode used parameters so that they are checked when another effect is instanced
                        effectCompilerResults.Add(compilerResult);
                    }
                }
            }

            foreach (var message in compilerResult.Messages)
            {
                Log.Log(message);
            }

            return compilerResult;
        }

        private void UpdateEffects()
        {
            lock (recentlyModifiedShaders)
            {
                if (recentlyModifiedShaders.Count == 0)
                    return;

                // Clear cache for recently modified shaders
                compiler.ResetCache(recentlyModifiedShaders);

                var bytecodeRemoved = new List<EffectBytecode>();

                lock (cachedEffects)
                {
                    foreach (var shaderSourceName in recentlyModifiedShaders)
                    {
                        // TODO: Cache keys in a HashSet instead of ToHashSet
                        var bytecodes = new HashSet<EffectBytecode>(cachedEffects.Keys);
                        foreach (var bytecode in bytecodes)
                        {
                            if (bytecode.HashSources.ContainsKey(shaderSourceName))
                            {
                                bytecodeRemoved.Add(bytecode);

                                // Dispose previous effect
                                var effect = cachedEffects[bytecode];
                                // TODO: Should be reference counted instead of disposed
                                effect.Dispose();
                                effect.SourceChanged = true;

                                // Remove effect from cache
                                cachedEffects.Remove(bytecode);
                            }
                        }
                    }
                }

                lock (earlyCompilerCache)
                {
                    foreach (var effectCompilerResults in earlyCompilerCache.Values)
                    {
                        foreach (var bytecode in bytecodeRemoved)
                        {
                            effectCompilerResults.RemoveAll(results => results.Bytecode.GetCurrentResult().Bytecode == bytecode);
                        }
                    }
                }

                recentlyModifiedShaders.Clear();
            }
        }

        private void FileModifiedEvent(object sender, FileEvent e)
        {
            if (e.ChangeType == FileEventChangeType.Changed ||
                e.ChangeType == FileEventChangeType.Renamed)
            {
                lock (recentlyModifiedShaders)
                {
                    recentlyModifiedShaders.Add(Path.GetFileNameWithoutExtension(e.Name));
                }
            }
        }

        /// <summary>
        ///   Gets the shader from the database based on the parameters used for its compilation.
        /// </summary>
        /// <param name="effectName">Name of the effect.</param>
        /// <param name="parameters">The compilation parameters.</param>
        /// <returns>The effect bytecode, if found.</returns>
        protected CompilerResults GetShaderFromParameters(string effectName, CompilerParameters parameters)
        {
            lock (earlyCompilerCache)
            {
                if (!earlyCompilerCache.TryGetValue(effectName, out List<CompilerResults> compilerResultsList))
                    return null;

                // Compiler Parameters are supposed to be created in the same order every time, so we just check if they were created in the same order (ParameterKeyInfos) with same values (ObjectValues)

                // TODO: GRAPHICS REFACTOR we could probably compute a hash for faster lookup
                foreach (var compiledResults in compilerResultsList)
                {
                    var compiledParameters = compiledResults.SourceParameters;

                    var compiledParameterKeyInfos = compiledParameters.ParameterKeyInfos;
                    var parameterKeyInfos = parameters.ParameterKeyInfos;

                    // Early check
                    if (parameterKeyInfos.Count != compiledParameterKeyInfos.Count)
                        continue;

                    for (int index = 0; index < parameterKeyInfos.Count; ++index)
                    {
                        var parameterKeyInfo = parameterKeyInfos[index];
                        var compiledParameterKeyInfo = compiledParameterKeyInfos[index];

                        if (parameterKeyInfo != compiledParameterKeyInfo)
                            goto different;

                        // Should not happen in practice (CompilerParameters should only consist of permutation values)
                        if (parameterKeyInfo.Key.Type != ParameterKeyType.Permutation)
                            continue;

                        for (int i = 0; i < parameterKeyInfo.Count; ++i)
                        {
                            var object1 = parameters.ObjectValues[parameterKeyInfo.BindingSlot + i];
                            var object2 = compiledParameters.ObjectValues[compiledParameterKeyInfo.BindingSlot + i];
                            if (object1 is null && object2 is null)
                                continue;
                            if ((object1 is null && object2 != null) ||
                                (object2 is null && object1 != null))
                                goto different;
                            if (!object1.Equals(object2))
                                goto different;
                        }
                    }

                    return compiledResults;

                different:
                    continue;
                }
            }

            return null;
        }
    }
}
