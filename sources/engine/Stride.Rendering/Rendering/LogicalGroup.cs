// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Storage;

namespace Stride.Rendering
{
    /// <summary>
    ///   Defines a group of descriptors and constant buffers that are updated together.
    /// </summary>
    /// <remarks>
    ///   It can be declared in shader using the syntax <c>cbuffer PerView.LogicalGroupName</c>
    ///   (also works with <c>rgroup</c>).
    /// </remarks>
    public struct LogicalGroup
    {
        public ObjectId Hash;

        public int DescriptorEntryStart;
        public int DescriptorEntryCount;
        public int DescriptorSlotStart;
        public int DescriptorSlotCount;

        public int ConstantBufferMemberStart;
        public int ConstantBufferMemberCount;
        public int ConstantBufferOffset;
        public int ConstantBufferSize;
    }
}
