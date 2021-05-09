// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Provides a mechanism to register logging messages and information about the functioning of a process,
    ///   program or operation.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///   Gets the module this logger refers to.
        /// </summary>
        /// <value>The module.</value>
        /// <remarks>
        ///   The module is an identifier for a logical part of the system. It can be a class name, a namespace or a regular string
        ///   not linked to a code hierarchy.
        /// </remarks>
        string Module { get; }

        /// <summary>
        ///   Logs the specified log message.
        /// </summary>
        /// <param name="logMessage">The log message.</param>
        void Log(ILogMessage logMessage);
    }
}
