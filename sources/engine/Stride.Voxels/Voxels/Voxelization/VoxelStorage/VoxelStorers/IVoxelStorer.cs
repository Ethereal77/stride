// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
