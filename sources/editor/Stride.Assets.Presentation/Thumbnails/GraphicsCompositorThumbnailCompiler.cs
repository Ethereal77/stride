// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets.Compiler;
using Stride.Assets.Presentation.Resources.Thumbnails;
using Stride.Assets.Rendering;
using Stride.Editor.Thumbnails;

namespace Stride.Assets.Presentation.Thumbnails
{
    [AssetCompiler(typeof(GraphicsCompositorAsset), typeof(ThumbnailCompilationContext))]
    public class GraphicsCompositorThumbnailCompiler : StaticThumbnailCompiler<GraphicsCompositorAsset>
    {
        public GraphicsCompositorThumbnailCompiler()
            : base(StaticThumbnails.GraphicsCompositorThumbnail)
        {
        }
    }
}
