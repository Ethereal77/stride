// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Specifies how the operating system should open a file.
    /// </summary>
    public enum VirtualFileMode
    {
        /// <summary>
        ///   Specifies that the operating system should create a new file.
        /// </summary>
        /// <remarks>
        ///   If the file already exists, an <see cref="IOException"/> exception is thrown.
        /// </remarks>
        CreateNew = 1,

        /// <summary>
        ///   Specifies that the operating system should create a new file. If the file already
        ///   exists, it will be overwritten.
        /// </summary>
        /// <remarks>
        ///   This is equivalent to requesting that if the file does not exist, use <see cref="CreateNew"/>;
        ///   otherwise, use <see cref="Truncate"/>.
        /// </remarks>
        Create = 2,

        /// <summary>
        ///   Specifies that the operating system should open an existing file. The ability to open the file is
        ///   dependent on the value specified by the <see cref="VirtualFileAccess"/> enumeration.
        /// </summary>
        /// <remarks>
        ///   A <see cref="FileNotFoundException"/> exception is thrown if the file does not exist.
        /// </remarks>
        Open = 3,

        /// <summary>
        ///   Specifies that the operating system should open a file if it exists; otherwise, a new file should be
        ///   created.
        /// </summary>
        /// <remarks>
        ///   This is equivalent to requesting that if the file does not exist, use <see cref="CreateNew"/>;
        ///   otherwise, use <see cref="Open"/>.
        ///   <para/>
        ///   A <see cref="FileNotFoundException"/> exception is thrown if the file does not exist.
        /// </remarks>
        OpenOrCreate = 4,

        /// <summary>
        ///   Specifies that the operating system should open an existing file. When the file
        ///   is opened, it should be truncated so that its size is zero bytes.
        /// </summary>
        /// <remarks>
        ///   A <see cref="FileNotFoundException"/> exception is thrown if the file does not exist.
        ///   <para/>
        ///   Attempts to read from a file opened with <see cref="Truncate"/> cause an <see cref="ArgumentException"/>
        ///   exception. The file must be opened with <see cref="VirtualFileAccess.Write"/>.
        /// </remarks>
        Truncate = 5,

        /// <summary>
        ///   Opens the file if it exists and seeks to the end of the file, or creates a new file.
        /// </summary>
        /// <remarks>
        ///   A <see cref="FileNotFoundException"/> exception is thrown if the file does not exist.
        ///   <para/>
        ///   <see cref="Append"/> can be used only in conjunction with <see cref="VirtualFileAccess.Write"/>.
        ///   <para/>
        ///   Trying to seek to a position before the end of the file throws an <see cref="IOException"/>
        ///   exception, and any attempt to read fails and throws a <see cref="NotSupportedException"/> exception.
        /// </remarks>
        Append = 6
    }
}
