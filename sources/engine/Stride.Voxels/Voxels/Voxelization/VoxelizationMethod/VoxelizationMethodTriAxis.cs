// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Shaders;
using Stride.Graphics;

namespace Stride.Rendering.Voxels
{
    /// <summary>
    ///   Renders the scene 3 times from different axis to generate all the fragments, no geometry shader needed. Shadows don't work currently.
    /// </summary>
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    [Display("Tri Axis")]
    public class VoxelizationMethodTriAxis : IVoxelizationMethod
    {
        VoxelizationMethodSingleAxis axisX = new() { VoxelizationAxis = VoxelizationMethodSingleAxis.Axis.X };
        VoxelizationMethodSingleAxis axisY = new() { VoxelizationAxis = VoxelizationMethodSingleAxis.Axis.Y };
        VoxelizationMethodSingleAxis axisZ = new() { VoxelizationAxis = VoxelizationMethodSingleAxis.Axis.Z };

        public MultisampleCount MultisampleCount = MultisampleCount.X8;

        public void CollectVoxelizationPasses(VoxelizationPassList passList, IVoxelStorer storer, Matrix view, Vector3 resolution, VoxelAttribute attr, VoxelizationStage stage, bool output, bool shadows)
        {
            axisX.MultisampleCount = MultisampleCount;
            axisY.MultisampleCount = MultisampleCount;
            axisZ.MultisampleCount = MultisampleCount;

            axisX.CollectVoxelizationPasses(passList, storer, view, resolution, attr, stage, output, shadows);
            axisY.CollectVoxelizationPasses(passList, storer, view, resolution, attr, stage, output, shadows);
            axisZ.CollectVoxelizationPasses(passList, storer, view, resolution, attr, stage, output, shadows);
        }

        public void Render(VoxelStorageContext storageContext, RenderDrawContext drawContext, RenderView view) { }

        public void Reset() { }

        public ShaderSource GetVoxelizationShader() => null;

        public bool RequireGeometryShader() => false;

        public int GeometryShaderOutputCount() => 3;

        public bool CanShareRenderStage(IVoxelizationMethod obj)
        {
            if (obj is not VoxelizationMethodSingleAxis method)
                return false;

            if (method.MultisampleCount != MultisampleCount)
                return false;

            return true;
        }
    }
}
