// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    /// <summary>
    /// Handle used to query what's the actual offset of a given variable in a constant buffer, through <see cref="ResourceGroupLayout.GetConstantBufferOffset"/>.
    /// </summary>
    public struct ConstantBufferOffsetReference
    {
        public static readonly ConstantBufferOffsetReference Invalid = new ConstantBufferOffsetReference(-1);

        internal int Index;

        internal ConstantBufferOffsetReference(int index)
        {
            Index = index;
        }
    }
}
