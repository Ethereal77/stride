// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
