// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.IO
{
    /// <summary>
    /// Describes the different type of streams.
    /// </summary>
    [Flags]
    public enum StreamFlags
    {
        /// <summary>
        /// Returns the default underlying stream without any alterations. Can be a seek-able stream or not depending on the file.
        /// </summary>
        None,

        /// <summary>
        /// A stream in which we can seek
        /// </summary>
        Seekable,
    }
}
