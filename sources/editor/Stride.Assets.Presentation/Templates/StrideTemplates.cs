// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Templates;

namespace Stride.Assets.Presentation.Templates
{
    internal static class StrideTemplates
    {
        public static void Register()
        {
            // TODO: Attribute-based auto registration would be better I guess
            TemplateManager.RegisterGenerator(TemplateSampleGenerator.Default);
            TemplateManager.RegisterGenerator(NewGameTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(ProjectLibraryTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(UpdatePlatformsTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(AssetFactoryTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(AssetFromFileTemplateGenerator.Default);
            // Specific asset templates must be registered after AssetFactoryTemplateGenerator
            TemplateManager.RegisterGenerator(HeightmapFactoryTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(ColliderShapeHullFactoryTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(ProceduralModelFactoryTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(SkyboxFactoryTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(GraphicsCompositorTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(ScriptTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(SpriteSheetFromFileTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(ModelFromFileTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(SkeletonFromFileTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(AnimationFromFileTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(VideoFromFileTemplateGenerator.Default);
            TemplateManager.RegisterGenerator(SoundFromFileTemplateGenerator.Default);
        }
    }
}
