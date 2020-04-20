// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Shaders
{
    /// <summary>
    /// Describes the type of constant buffer.
    /// </summary>
    [DataContract]
    public enum ConstantBufferType
    {
        /// <summary>
        /// An unknown buffer.
        /// </summary>
        Unknown,

        /// <summary>
        /// A standard constant buffer
        /// </summary>
        ConstantBuffer,

        /// <summary>
        /// A texture buffer
        /// </summary>
        TextureBuffer,
    }
}
