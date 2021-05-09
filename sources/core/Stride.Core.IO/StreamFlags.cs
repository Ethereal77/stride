// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.IO
{
    /// <summary>
    ///   Describes the different type of streams that can be opened.
    /// </summary>
    [Flags]
    public enum StreamFlags
    {
        /// <summary>
        ///   Returns the default underlying stream without any alterations.
        ///   Can be a seek-able stream or not depending on the file.
        /// </summary>
        None,

        /// <summary>
        ///   Returns a stream in which we can seek for random access to the data.
        /// </summary>
        Seekable
    }
}
