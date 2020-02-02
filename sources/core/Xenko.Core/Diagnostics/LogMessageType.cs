// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Diagnostics
{
    /// <summary>
    /// Type of a <see cref="LogMessage" />.
    /// </summary>
    [DataContract]
    public enum LogMessageType
    {
        /// <summary>
        /// A debug message (level 0).
        /// </summary>
        Debug = 0,

        /// <summary>
        /// A verbose message (level 1).
        /// </summary>
        Verbose = 1,

        /// <summary>
        /// An regular info message (level 2).
        /// </summary>
        Info = 2,

        /// <summary>
        /// A warning message (level 3).
        /// </summary>
        Warning = 3,

        /// <summary>
        /// An error message (level 4).
        /// </summary>
        Error = 4,

        /// <summary>
        /// A Fatal error message (level 5).
        /// </summary>
        Fatal = 5,
    }
}
