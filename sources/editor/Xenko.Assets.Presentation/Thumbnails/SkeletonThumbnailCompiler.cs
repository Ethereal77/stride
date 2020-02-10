// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Compiler;
using Xenko.Assets.Models;
using Xenko.Assets.Presentation.Resources.Thumbnails;
using Xenko.Editor.Thumbnails;

namespace Xenko.Assets.Presentation.Thumbnails
{
    [AssetCompiler(typeof(SkeletonAsset), typeof(ThumbnailCompilationContext))]
    public class SkeletonThumbnailCompiler : StaticThumbnailCompiler<SkeletonAsset>
    {
        public SkeletonThumbnailCompiler()
            : base(StaticThumbnails.SkeletonThumbnail)
        {
        }
    }
}
