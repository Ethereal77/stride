// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Engine;
using Stride.Rendering.Lights;

namespace Stride.Rendering.Shadows
{
    /// <summary>
    /// Interface to render a shadow map.
    /// </summary>
    public interface ILightShadowMapRenderer : ILightShadowRenderer
    {
        RenderStage ShadowCasterRenderStage { get; }

        LightShadowType GetShadowType(LightShadowMap lightShadowMap);

        ILightShadowMapShaderGroupData CreateShaderGroupData(LightShadowType shadowType);

        void Collect(RenderContext context, RenderView sourceView, LightShadowMapTexture lightShadowMap);
        
        void ApplyViewParameters(RenderDrawContext context, ParameterCollection parameters, LightShadowMapTexture shadowMapTexture);

        LightShadowMapTexture CreateShadowMapTexture(RenderView renderView, RenderLight renderLight, IDirectLight light, int shadowMapSize);
    }
}
