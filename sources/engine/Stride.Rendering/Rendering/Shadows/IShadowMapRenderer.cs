// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering.Lights;

namespace Stride.Rendering.Shadows
{
    /// <summary>
    /// Render shadow maps; should be set on <see cref="ForwardLightingRenderFeature.ShadowMapRenderer"/>.
    /// </summary>
    public interface IShadowMapRenderer
    {
        RenderSystem RenderSystem { get; set; }

        HashSet<RenderView> RenderViewsWithShadows { get; }

        List<ILightShadowMapRenderer> Renderers { get; }

        LightShadowMapTexture FindShadowMap(RenderView renderView, RenderLight light);

        void Collect(RenderContext context, Dictionary<RenderView, ForwardLightingRenderFeature.RenderViewLightData> renderViewLightDatas);

        void Draw(RenderDrawContext drawContext);

        void PrepareAtlasAsRenderTargets(CommandList commandList);

        void PrepareAtlasAsShaderResourceViews(CommandList commandList);

        void Flush(RenderDrawContext context);
    }
}
