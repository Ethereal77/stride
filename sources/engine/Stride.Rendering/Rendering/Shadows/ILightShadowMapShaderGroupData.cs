// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Rendering.Lights;
using Stride.Shaders;

namespace Stride.Rendering.Shadows
{
    public interface ILightShadowMapShaderGroupData
    {
        void ApplyShader(ShaderMixinSource mixin);

        void UpdateLayout(string compositionName);

        void UpdateLightCount(int lightLastCount, int lightCurrentCount);

        void ApplyViewParameters(RenderDrawContext context, ParameterCollection parameters, FastListStruct<LightDynamicEntry> currentLights);

        void ApplyDrawParameters(RenderDrawContext context, ParameterCollection parameters, FastListStruct<LightDynamicEntry> currentLights, ref BoundingBoxExt boundingBox);
    }
}
