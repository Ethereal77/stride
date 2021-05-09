// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Stride.Graphics
{
    /// <summary>
    /// Provides a pointer to 2D data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DataRectangle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRectangle"/> class.
        /// </summary>
        /// <param name="dataPointer">The pointer to the data.</param>
        /// <param name="pitch">The stride.</param>
        public DataRectangle(IntPtr dataPointer, int pitch)
        {
            DataPointer = dataPointer;
            Pitch = pitch;
        }

        /// <summary>
        /// Gets or sets a pointer to the data.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public IntPtr DataPointer;

        /// <summary>
        /// Gets or sets the number of bytes per row.
        /// </summary>
        /// <value>
        /// The row pitch in bytes.
        /// </value>
        public int Pitch;
    }
}
