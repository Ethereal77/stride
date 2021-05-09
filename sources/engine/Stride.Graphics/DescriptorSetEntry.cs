// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
{
    /// <summary>
    /// Used internally to store descriptor entries.
    /// </summary>
    internal struct DescriptorSetEntry
    {
        public object Value;

        /// <summary>
        /// The offset, shared parameter for either cbuffer or unordered access view.
        /// Describes the cbuffer offset or the initial counter offset value for UAVs of compute shaders.
        /// </summary>
        public int Offset;
        public int Size;

        public DescriptorSetEntry(object value, int offset, int size)
        {
            Value = value;
            Offset = offset;
            Size = size;
        }
    }
}
