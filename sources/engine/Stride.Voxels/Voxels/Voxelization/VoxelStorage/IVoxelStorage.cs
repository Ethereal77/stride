// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2019 Sean Boettger <sean@whypenguins.com>
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Voxels
{
    public interface IVoxelStorage
    {
        void UpdateFromContext(VoxelStorageContext context);
        void CollectVoxelizationPasses(ProcessedVoxelVolume data, VoxelStorageContext storageContext);

        int RequestTempStorage(int count);
        void UpdateTexture(VoxelStorageContext context, ref IVoxelStorageTexture texture, Stride.Graphics.PixelFormat pixelFormat, int layoutSize);
        void UpdateTempStorage(VoxelStorageContext context);
        void PostProcess(VoxelStorageContext context, RenderDrawContext drawContext, ProcessedVoxelVolume data);
    }
}
