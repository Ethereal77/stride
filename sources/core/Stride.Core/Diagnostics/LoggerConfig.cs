// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Diagnostics
{
    /// <summary>
    /// Configuration for <see cref="GlobalLogger"/>.
    /// </summary>
    public class LoggerConfig
    {
        /// <summary>
        /// Gets or sets the minimum level to allow logging.
        /// </summary>
        /// <value>The level.</value>
        public LogMessageType Level { get; set; }
    }
}
