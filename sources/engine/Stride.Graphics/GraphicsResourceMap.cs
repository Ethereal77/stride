// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    /// <summary>
    /// Structured returned by <see cref="GraphicsDeviceContext.Map"/>.
    /// </summary>
    public struct GraphicsResourceMap
    {
        /// <summary>
        /// Pointer to the data of the <see cref="GraphicsResource"/> being mapped into the CPU memory.
        /// </summary>
        /// <remarks>
        /// If <see cref="GraphicsProfile"/> is set to low, data are aligned to 4 bytes else alignment is 16 bytes.
        /// </remarks>
        public IntPtr DataPointer;

        /// <summary>
        /// The row pitch, or width, or physical size (in bytes) of the data.
        /// </summary>
        public int RowPitch;

        /// <summary>
        /// The depth pitch, or width, or physical size (in bytes)of the data.
        /// </summary>
        public int DepthPitch;
    }
}
