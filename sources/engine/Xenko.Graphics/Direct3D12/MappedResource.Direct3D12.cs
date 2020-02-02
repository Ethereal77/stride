// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_GRAPHICS_API_DIRECT3D12

namespace Xenko.Graphics
{
    public partial struct MappedResource
    {
        internal SharpDX.Direct3D12.Resource UploadResource;
        internal int UploadOffset;
    }
}
#endif
