// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Diagnostics
{
    /// <summary>
    /// Interface for logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the module this logger refers to.
        /// </summary>
        /// <value>The module.</value>
        string Module { get; }

        /// <summary>
        /// Logs the specified log message.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        void Log(ILogMessage logMessage);
    }
}
