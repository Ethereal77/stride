// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public interface IVoxelStorer
    {
        void PostProcess(VoxelStorageContext context, RenderDrawContext drawContext, ProcessedVoxelVolume data);


        ShaderSource GetVoxelizationShader(VoxelizationPass pass, ProcessedVoxelVolume data);

        void ApplyVoxelizationParameters(ParameterCollection param);

        void UpdateVoxelizationLayout(string compositionName);


        bool RequireGeometryShader();

        int GeometryShaderOutputCount();

        bool CanShareRenderStage(IVoxelStorer storer);
    }
}
