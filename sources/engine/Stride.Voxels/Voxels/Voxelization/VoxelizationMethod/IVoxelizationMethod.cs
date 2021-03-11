// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;
using Stride.Shaders;

namespace Stride.Rendering.Voxels
{
    public interface IVoxelizationMethod
    {
        void Reset();

        void CollectVoxelizationPasses(VoxelizationPassList passList, IVoxelStorer storer, Matrix view, Vector3 resolution, VoxelAttribute attr, VoxelizationStage stage, bool output, bool shadows);

        void Render(VoxelStorageContext storageContext, RenderDrawContext drawContext, RenderView view);


        bool RequireGeometryShader();

        int GeometryShaderOutputCount();


        ShaderSource GetVoxelizationShader();

        bool CanShareRenderStage(IVoxelizationMethod method);
    }
}
