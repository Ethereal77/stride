// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Graphics
{
    /// <summary>
    /// Multisample count level.
    /// </summary>
    public enum MultisampleCount
    {
        /// <summary>
        /// No multisample.
        /// </summary>
        None = 1,

        /// <summary>
        /// Multisample count of 2 pixels.
        /// </summary>
        X2 = 2,

        /// <summary>
        /// Multisample count of 4 pixels.
        /// </summary>
        X4 = 4,

        /// <summary>
        /// Multisample count of 8 pixels.
        /// </summary>
        X8 = 8,
    }
}
