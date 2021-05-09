// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
{
    /// <summary>
    /// Contains resources and a constant buffer, that usually change at a given frequency.
    /// </summary>
    public class ResourceGroup
    {
        /// <summary>
        /// Resources.
        /// </summary>
        public DescriptorSet DescriptorSet;

        /// <summary>
        /// Constant buffer.
        /// </summary>
        public BufferPoolAllocationResult ConstantBuffer;
    }
}
