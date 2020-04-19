// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets;
using Xenko.Core.Assets.Compiler;
using Xenko.Core.IO;
using Xenko.Assets;
using Xenko.Graphics;

namespace Xenko.Editor.Thumbnails
{
    /// <summary>
    /// The base class for a static thumbnail compiler
    /// </summary>
    /// <typeparam name="T">The type of asset taken in charge by the compiler</typeparam>
    public class StaticThumbnailCompiler<T> : ThumbnailCompilerBase<T> where T : Asset
    {
        private readonly byte[] staticImageData;

        public StaticThumbnailCompiler(byte[] staticImageData)
        {
            this.staticImageData = staticImageData;
            IsStatic = true;
        }

        protected override void CompileThumbnail(ThumbnailCompilerContext context, string thumbnailStorageUrl, AssetItem assetItem, Package originalPackage, AssetCompilerResult result)
        {
            var gameSettings = context.GetGameSettingsAsset();
            var renderingSettings = gameSettings.GetOrCreate<RenderingSettings>();
            result.BuildSteps.Add(new ThumbnailBuildStep(new StaticThumbnailCommand<T>(thumbnailStorageUrl, staticImageData, context.ThumbnailResolution, renderingSettings.ColorSpace == ColorSpace.Linear, assetItem.Package)));
        }

        protected override string BuildThumbnailStoreName(UFile assetUrl)
        {
            return ThumbnailStorageNamePrefix + typeof(T).Name;
        }
    }
}
