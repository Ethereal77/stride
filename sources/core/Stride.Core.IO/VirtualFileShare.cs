// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Contains constants for controlling the kind of access other virtual <see cref="Stream"/>
    ///   objects can have to the same file.
    /// </summary>
    [Flags]
    public enum VirtualFileShare : uint
    {
        /// <summary>
        ///   Declines sharing of the current file. Any request to open the file (by this process
        ///   or another process) will fail until the file is closed.
        /// </summary>
        None = 0,

        /// <summary>
        ///   Allows subsequent opening of the file for reading. If this flag is not specified,
        ///   any request to open the file for reading (by this process or another process)
        ///   will fail until the file is closed. However, even if this flag is specified,
        ///   additional permissions might still be needed to access the file.
        /// </summary>
        Read = 1,

        /// <summary>
        ///   Allows subsequent opening of the file for writing. If this flag is not specified,
        ///   any request to open the file for writing (by this process or another process)
        ///   will fail until the file is closed. However, even if this flag is specified,
        ///   additional permissions might still be needed to access the file.
        /// </summary>
        Write = 2,

        /// <summary>
        ///   Allows subsequent opening of the file for reading or writing. If this flag is
        ///   not specified, any request to open the file for reading or writing (by this process
        ///   or another process) will fail until the file is closed. However, even if this
        ///   flag is specified, additional permissions might still be needed to access the file.
        /// </summary>
        ReadWrite = 3,

        /// <summary>
        ///   Allows subsequent deleting of a file.
        /// </summary>
        Delete = 4
    }
}
