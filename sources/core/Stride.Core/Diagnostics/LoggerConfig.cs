// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Contains configuration values for <see cref="GlobalLogger"/>.
    /// </summary>
    [DataContract("GlobalLoggerConfig")]
    public class LoggerConfig
    {
        /// <summary>
        ///   Gets or sets the minimum log level to allow.
        /// </summary>
        /// <value>The level.</value>
        public LogMessageType Level { get; set; }
    }
}
