// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D12

using System.Collections.Generic;

using SharpDX.Direct3D12;

namespace Stride.Graphics
{
    public partial struct CompiledCommandList
    {
        internal CommandList Builder;
        internal GraphicsCommandList NativeCommandList;
        internal CommandAllocator NativeCommandAllocator;
        internal List<DescriptorHeap> SrvHeaps;
        internal List<DescriptorHeap> SamplerHeaps;
        internal List<GraphicsResource> StagingResources;
    }
}
#endif
