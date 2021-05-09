// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    /// <summary>
    /// Specifies the buffer to use when calling Clear.
    /// </summary>
    [Flags]
    public enum DepthStencilClearOptions
    {
        /// <summary>
        /// A depth buffer.
        /// </summary>
        DepthBuffer = 1,
        /// <summary>
        /// A stencil buffer.
        /// </summary>
        Stencil = 2,
    }
}
