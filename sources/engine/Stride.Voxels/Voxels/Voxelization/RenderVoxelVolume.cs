// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Mathematics;
using Stride.Rendering.Voxels.Debug;

namespace Stride.Rendering.Voxels
{
    public class DataVoxelVolume
    {
        public Vector3 VolumeSize;
        public Vector3 VolumeTranslation;
        public float AproxVoxelSize;
        public bool Voxelize;

        public bool VoxelGridSnapping;
        public bool VisualizeVoxels;
        public VoxelAttribute VisualizationAttribute;
        public IVoxelVisualization VoxelVisualization;

        public List<VoxelAttribute> Attributes = new List<VoxelAttribute>();

        public IVoxelStorage Storage;
        public IVoxelizationMethod VoxelizationMethod;
    }

    public enum VoxelizationStage
    {
        Initial,
        Post
    }

    public struct AttributeStream
    {
        public VoxelAttribute Attribute;
        public VoxelizationStage Stage;
        public bool Output;

        public AttributeStream(VoxelAttribute attribute, VoxelizationStage stage, bool output)
        {
            Attribute = attribute;
            Stage = stage;
            Output = output;
        }
    }

    public class ProcessedVoxelVolume
    {
        public bool Voxelize;
        public bool VisualizeVoxels;
        public VoxelAttribute VisualizationAttribute;
        public IVoxelVisualization VoxelVisualization;

        public IVoxelStorage Storage;
        public IVoxelizationMethod VoxelizationMethod;
        public VoxelStorageContext StorageContext;

        public VoxelizationPassList passList = new VoxelizationPassList();

        public List<List<VoxelizationPass>> groupedPasses = new List<List<VoxelizationPass>>();

        public List<VoxelAttribute> OutputAttributes = new List<VoxelAttribute>();
        public List<AttributeStream> Attributes = new List<AttributeStream>();
    }
}
