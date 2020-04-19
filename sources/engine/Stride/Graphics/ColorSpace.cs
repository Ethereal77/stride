// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Graphics
{
    /// <summary>
    /// The colorspace used for textures, materials, lighting...
    /// </summary>
    [DataContract("ColorSpace")]
    public enum ColorSpace
    {
        /// <summary>
        /// Use a linear colorspace.
        /// </summary>
        Linear,

        /// <summary>
        /// Use a gamma colorspace.
        /// </summary>
        Gamma,
    }
}
