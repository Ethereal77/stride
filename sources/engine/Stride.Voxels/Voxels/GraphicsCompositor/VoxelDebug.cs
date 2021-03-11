// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Stride.Core;
using Stride.Graphics;
using Stride.Rendering.Images;

namespace Stride.Rendering.Voxels.Debug
{
    [DataContract("VoxelDebug")]
    public class VoxelDebug : ImageEffect
    {
        [DataMemberIgnore]
        public IVoxelRenderer VoxelRenderer;
        protected override void InitializeCore()
        {
            base.InitializeCore();
        }
        protected override void DrawCore(RenderDrawContext context)
        {
            if (!Initialized)
                Initialize(context.RenderContext);

            if (VoxelRenderer is null)
                return;

            Dictionary<VoxelVolumeComponent, ProcessedVoxelVolume> precessedVolumes = VoxelRenderer.GetProcessedVolumes();
            if (precessedVolumes is null)
                return;

            foreach (var volume in precessedVolumes)
            {
                var data = volume.Value;

                if (!data.VisualizeVoxels ||
                    data.VoxelVisualization is null ||
                    data.VisualizationAttribute is null)
                    continue;

                ImageEffectShader shader = data.VoxelVisualization.GetShader(context, data.VisualizationAttribute);

                if (shader is null)
                    continue;

                shader.SetOutput(GetSafeOutput(0));

                shader.Draw(context);
            }
        }

        public void Draw(RenderDrawContext drawContext, Texture output)
        {
            SetOutput(output);
            DrawCore(drawContext);
        }
    }
}
