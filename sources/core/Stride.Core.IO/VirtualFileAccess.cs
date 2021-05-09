// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Defines constants for read, write, or read/write access to a file.
    /// </summary>
    [Flags]
    public enum VirtualFileAccess : uint
    {
        /// <summary>
        ///   Read access to the file. Data can be read from the file. Combine with <see cref="Write"/> for read/write access.
        /// </summary>
        Read = 1,

        /// <summary>
        ///   Write access to the file. Data can be written to the file. Combine with <see cref="Read"/> for read/write access.
        /// </summary>
        Write = 2,

        /// <summary>
        ///   Read and write access to the file. Data can be written to and read from the file.
        /// </summary>
        ReadWrite = Read | Write
    }
}
