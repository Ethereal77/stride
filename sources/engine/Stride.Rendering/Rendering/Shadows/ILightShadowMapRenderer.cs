// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;
using Xenko.Rendering.Lights;

namespace Xenko.Rendering.Shadows
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
