// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Rendering;
using Stride.Shaders;

namespace Stride.Graphics
{
    // D3D11 version
    /// <summary>
    /// Defines a list of descriptor layout. This is used to allocate a <see cref="DescriptorSet"/>.
    /// </summary>
    public partial class DescriptorSetLayout : GraphicsResourceBase
    {
        public static DescriptorSetLayout New(GraphicsDevice device, DescriptorSetLayoutBuilder builder)
        {
            return new DescriptorSetLayout(device, builder);
        }

#if STRIDE_GRAPHICS_API_DIRECT3D11
        internal readonly int ElementCount;
        internal readonly DescriptorSetLayoutBuilder.Entry[] Entries;

        private DescriptorSetLayout(GraphicsDevice device, DescriptorSetLayoutBuilder builder)
        {
            ElementCount = builder.ElementCount;
            Entries = builder.Entries.ToArray();
        }
#endif
    }
}
