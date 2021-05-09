// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.BuildEngine;
using Stride.Core;
using Stride.Core.IO;
using Stride.Core.Serialization;
using Stride.Core.Serialization.Contents;
using Stride.Data;
using Stride.Engine;
using Stride.Engine.Design;
using Stride.Graphics;
using Stride.Rendering.Compositing;

namespace Stride.Assets
{
    [AssetCompiler(typeof(GameSettingsAsset), typeof(AssetCompilationContext))]
    public class GameSettingsAssetCompiler : AssetCompilerBase
    {
        protected override void Prepare(AssetCompilerContext context, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var asset = (GameSettingsAsset)assetItem.Asset;
            // TODO: We should ignore game settings stored in dependencies
            result.BuildSteps = new AssetBuildStep(assetItem);
            result.BuildSteps.Add(new GameSettingsCompileCommand(targetUrlInStorage, assetItem.Package, context.Package, context.Platform, context.GetCompilationMode(), asset));
        }

        public override IEnumerable<Type> GetRuntimeTypes(AssetItem assetItem)
        {
            yield return typeof(Scene);
            yield return typeof(GraphicsCompositor);
            yield return typeof(Texture);
        }

        private class GameSettingsCompileCommand : AssetCommand<GameSettingsAsset>
        {
            private readonly PlatformType platform;
            private readonly CompilationMode compilationMode;
            private readonly Package package;
            private readonly Package entryPackage;
            public GameSettingsCompileCommand(string url, Package package, Package entryPackage, PlatformType platform, CompilationMode compilationMode, GameSettingsAsset asset)
                : base(url, asset, package)
            {
                this.package = package;
                this.entryPackage = entryPackage ?? package;
                this.platform = platform;
                this.compilationMode = compilationMode;
            }

            protected override void ComputeParameterHash(BinarySerializationWriter writer)
            {
                base.ComputeParameterHash(writer);

                // Hash used parameters from package
                writer.Write(package.Meta.Name);
                writer.Write(entryPackage.UserSettings.GetValue(GameUserSettings.Effect.EffectCompilation));
                writer.Write(entryPackage.UserSettings.GetValue(GameUserSettings.Effect.RecordUsedEffects));
                writer.Write(compilationMode);

                // Hash platform
                writer.Write(platform);
            }

            protected override Task<ResultStatus> DoCommandOverride(ICommandContext commandContext)
            {
                var result = new GameSettings
                {
                    PackageName = package.Meta.Name,
                    DefaultSceneUrl = Parameters.DefaultScene != null ? AttachedReferenceManager.GetUrl(Parameters.DefaultScene) : null,
                    DefaultGraphicsCompositorUrl = Parameters.GraphicsCompositor != null ? AttachedReferenceManager.GetUrl(Parameters.GraphicsCompositor) : null,
                    SplashScreenUrl = Parameters.SplashScreenTexture != null && (compilationMode == CompilationMode.Release || compilationMode == CompilationMode.AppStore) ? AttachedReferenceManager.GetUrl(Parameters.SplashScreenTexture) : null,
                    SplashScreenColor = Parameters.SplashScreenColor,
                    EffectCompilation = package.UserSettings.GetValue(GameUserSettings.Effect.EffectCompilation),
                    RecordUsedEffects = package.UserSettings.GetValue(GameUserSettings.Effect.RecordUsedEffects),
                    Configurations = new PlatformConfigurations(),
                    CompilationMode = compilationMode
                };

                //start from the default platform and go down overriding

                foreach (var configuration in Parameters.Defaults.Where(x => !x.OfflineOnly))
                {
                    result.Configurations.Configurations.Add(new ConfigurationOverride
                    {
                        Platforms = ConfigPlatforms.None,
                        SpecificFilter = -1,
                        Configuration = configuration
                    });
                }

                foreach (var configurationOverride in Parameters.Overrides.Where(x => x.Configuration != null && !x.Configuration.OfflineOnly))
                {
                    result.Configurations.Configurations.Add(configurationOverride);
                }

                result.Configurations.PlatformFilters = Parameters.PlatformFilters;

                //make sure we modify platform specific files to set the wanted orientation
                if (package.Container is SolutionProject solutionProject)
                    SetPlatformOrientation(solutionProject, Parameters.GetOrDefault<RenderingSettings>().DisplayOrientation);

                var assetManager = new ContentManager(MicrothreadLocalDatabases.ProviderService);
                assetManager.Save(Url, result);

                return Task.FromResult(ResultStatus.Successful);
            }
        }

        public static void SetPlatformOrientation(SolutionProject project, RequiredDisplayOrientation orientation)
        {
        }
    }
}
