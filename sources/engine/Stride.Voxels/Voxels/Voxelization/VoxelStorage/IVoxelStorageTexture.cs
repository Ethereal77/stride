// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public interface IVoxelStorageTexture
    {
        void UpdateVoxelizationLayout(string compositionName);

        void UpdateSamplingLayout(string compositionName);

        void ApplyVoxelizationParameters(ObjectParameterKey<Texture> MainKey, ParameterCollection parameters);

        void PostProcess(RenderDrawContext drawContext, ShaderSource[] mipmapShaders);

        ShaderClassSource GetSamplingShader();

        void ApplySamplingParameters(VoxelViewContext viewContext, ParameterCollection parameters);
    }
}
