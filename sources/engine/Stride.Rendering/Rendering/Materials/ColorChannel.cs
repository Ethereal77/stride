// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// A RGBA channel selected when performing texture sampling.
    /// </summary>
    [DataContract("ColorChannel")]
    public enum ColorChannel
    {
        /// <summary>
        /// The sampled color is returned as a float4(R, R, R, R)
        /// </summary>
        R,

        /// <summary>
        /// The sampled color is returned as a float4(G, G, G, G)
        /// </summary>
        G,

        /// <summary>
        /// The sampled color is returned as a float4(B, B, B, B)
        /// </summary>
        B,

        /// <summary>
        /// The sampled color is returned as a float4(A, A, A, A)
        /// </summary>
        A,
    }
}
