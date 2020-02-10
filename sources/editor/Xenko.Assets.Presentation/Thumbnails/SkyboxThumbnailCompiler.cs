// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Compiler;
using Xenko.Assets.Presentation.Resources.Thumbnails;
using Xenko.Assets.Skyboxes;
using Xenko.Editor.Thumbnails;

namespace Xenko.Assets.Presentation.Thumbnails
{
    [AssetCompiler(typeof(SkyboxAsset), typeof(ThumbnailCompilationContext))]
    public class SkyboxThumbnailCompiler : StaticThumbnailCompiler<SkyboxAsset>
    {
        public SkyboxThumbnailCompiler()
            : base(StaticThumbnails.SkyboxThumbnail)
        {
        }
    }
}
