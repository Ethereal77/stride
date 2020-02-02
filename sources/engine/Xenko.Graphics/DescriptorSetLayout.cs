// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Rendering;
using Xenko.Shaders;

namespace Xenko.Graphics
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

#if XENKO_GRAPHICS_API_DIRECT3D11
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
