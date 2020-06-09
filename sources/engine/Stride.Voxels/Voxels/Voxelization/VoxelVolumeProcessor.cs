// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Stride.Core.Mathematics;
using Stride.Rendering;
using Stride.Rendering.Voxels;

namespace Stride.Engine.Processors
{
    public class VoxelVolumeProcessor : EntityProcessor<VoxelVolumeComponent>, IEntityComponentRenderProcessor
    {
        private readonly Dictionary<VoxelVolumeComponent, DataVoxelVolume> renderVoxelVolumes = new Dictionary<VoxelVolumeComponent, DataVoxelVolume>();
        public Dictionary<VoxelVolumeComponent, ProcessedVoxelVolume> processedVoxelVolumes = new Dictionary<VoxelVolumeComponent, ProcessedVoxelVolume>();

        public VisibilityGroup VisibilityGroup { get; set; }
        public RenderGroup RenderGroup { get; set; }

        protected override void OnSystemAdd()
        {
            base.OnSystemAdd();

            VisibilityGroup.Tags.Set(VoxelRenderer.CurrentRenderVoxelVolumes, renderVoxelVolumes);
            VisibilityGroup.Tags.Set(VoxelRenderer.CurrentProcessedVoxelVolumes, processedVoxelVolumes);
            VisibilityGroup.Tags.Set(VoxelRenderFeature.CurrentProcessedVoxelVolumes, processedVoxelVolumes);
        }

        public override void Draw(RenderContext context)
        {
            RegenerateVoxelVolumes();
        }

        public ProcessedVoxelVolume GetProcessedVolumeForComponent(VoxelVolumeComponent component)
        {
            if (!processedVoxelVolumes.TryGetValue(component, out ProcessedVoxelVolume data))
                return null;

            return data;
        }

        public DataVoxelVolume GetRenderVolumeForComponent(VoxelVolumeComponent component)
        {
            if (!renderVoxelVolumes.TryGetValue(component, out DataVoxelVolume data))
                return null;

            return data;
        }

        private void RegenerateVoxelVolumes()
        {
            renderVoxelVolumes.Clear();
            processedVoxelVolumes.Clear();

            foreach (var pair in ComponentDatas)
            {
                if (!pair.Key.Enabled)
                    continue;

                var volume = pair.Key;

                DataVoxelVolume data;
                renderVoxelVolumes.Add(volume, data = new DataVoxelVolume());
                processedVoxelVolumes.Add(volume, new ProcessedVoxelVolume());

                data.VolumeTranslation = volume.Entity.Transform.WorldMatrix.TranslationVector;
                data.VolumeSize = new Vector3(volume.VoxelVolumeSize);

                data.Voxelize = volume.Voxelize;
                data.AproxVoxelSize = volume.AproximateVoxelSize;

                data.VoxelGridSnapping = volume.VoxelGridSnapping;
                data.VisualizeVoxels = volume.VisualizeVoxels;
                data.VoxelVisualization = volume.Visualization;
                data.Attributes = volume.Attributes;
                data.Storage = volume.Storage;
                data.VoxelizationMethod = volume.VoxelizationMethod;

                if (volume.Attributes.Count > volume.VisualizeIndex)
                {
                    data.VisualizationAttribute = volume.Attributes[volume.VisualizeIndex];
                }
                else
                {
                    data.VisualizationAttribute = null;
                }
            }
        }
    }
}
