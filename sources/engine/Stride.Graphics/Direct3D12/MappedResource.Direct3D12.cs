// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_GRAPHICS_API_DIRECT3D12

namespace Stride.Graphics
{
    public partial struct MappedResource
    {
        internal SharpDX.Direct3D12.Resource UploadResource;
        internal int UploadOffset;
    }
}

#endif
