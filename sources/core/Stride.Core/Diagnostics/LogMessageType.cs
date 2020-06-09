// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Defines the different types of messages that can represent a <see cref="ILogMessage"/>.
    /// </summary>
    [DataContract]
    public enum LogMessageType
    {
        /// <summary>
        ///   The message is intended for debugging information (level 0).
        /// </summary>
        Debug = 0,

        /// <summary>
        ///   The message is for informational verbose purposes (level 1).
        /// </summary>
        Verbose = 1,

        /// <summary>
        ///   The message is a regular informational message (level 2).
        /// </summary>
        Info = 2,

        /// <summary>
        ///   The message is a warning (level 3).
        /// </summary>
        Warning = 3,

        /// <summary>
        ///   The message informs of a recoverable error that has occurred (level 4).
        /// </summary>
        Error = 4,

        /// <summary>
        ///   The message reports a fatal unrecoverable error (level 5).
        /// </summary>
        Fatal = 5
    }
}
