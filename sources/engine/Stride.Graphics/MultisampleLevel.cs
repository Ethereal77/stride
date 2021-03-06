// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
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
