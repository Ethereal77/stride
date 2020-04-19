// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Diagnostics
{
    /// <summary>
    /// Arguments of the <see cref="Logger.MessageLogged"/> event.
    /// </summary>
    public class MessageLoggedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLoggedEventArgs"/> class with a log message.
        /// </summary>
        /// <param name="message">The message that has been logged.</param>
        public MessageLoggedEventArgs(ILogMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message that has been logged.
        /// </summary>
        public ILogMessage Message { get; private set; }
    }
}
