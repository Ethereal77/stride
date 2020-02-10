// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.BuildEngine
{
    /// <summary>
    /// Status of a command.
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>
        /// The command has not finished yet
        /// </summary>
        NotProcessed,
        /// <summary>
        /// The command was successfully executed
        /// </summary>
        Successful,
        /// <summary>
        /// The command execution failed
        /// </summary>
        Failed,
        /// <summary>
        /// The command was started but cancelled, output is undeterminated
        /// </summary>
        Cancelled,
        /// <summary>
        /// A command may not be triggered if its input data haven't changed since the successful last execution
        /// </summary>
        NotTriggeredWasSuccessful,
        /// <summary>
        /// One of the prerequisite command failed and the command was not executed
        /// </summary>
        NotTriggeredPrerequisiteFailed,
    }
}
