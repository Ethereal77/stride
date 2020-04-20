// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Compiler;
using Stride.Assets.Models;
using Stride.Assets.Presentation.Resources.Thumbnails;
using Stride.Editor.Thumbnails;

namespace Stride.Assets.Presentation.Thumbnails
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
