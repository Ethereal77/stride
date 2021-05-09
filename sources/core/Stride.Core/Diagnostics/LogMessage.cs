// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Represents a log message used by the logging infrastructure and serves as the base for other log message types.
    /// </summary>
    /// <remarks>
    ///   This class can be derived in order to provide additional custom log information.
    /// </remarks>
    public class LogMessage : ILogMessage
    {
        /// <summary>
        ///   Gets or sets the module specified in the message.
        /// </summary>
        /// <value>The module.</value>
        /// <remarks>
        ///   The module is an identifier for a logical part of the system. It can be a class name, a namespace or a regular string
        ///   not linked to a code hierarchy.
        /// </remarks>
        public string Module { get; set; }

        /// <summary>
        ///   Gets or sets the type of this message.
        /// </summary>
        /// <value>The type of the message.</value>
        public LogMessageType Type { get; set; }

        /// <summary>
        ///   Gets or sets the text of the message.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the exception, in case this message represents an error or exception.
        /// </summary>
        /// <value><see cref="System.Exception"/> represented by this message.</value>
        public Exception Exception { get; set; }

        /// <summary>
        ///   Gets or sets the caller information of this message.
        /// </summary>
        /// <value>The caller information.</value>
        public CallerInfo CallerInfo { get; set; }

        /// <inheritdoc/>
        public ExceptionInfo ExceptionInfo => Exception != null ? new ExceptionInfo(Exception) : null;


        /// <summary>
        ///   Initializes a new instance of the <see cref="LogMessage" /> class.
        /// </summary>
        public LogMessage() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="LogMessage" /> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        public LogMessage(string module, LogMessageType type, string text)
        {
            Module = module;
            Type = type;
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage" /> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="callerInfo">The caller info.</param>
        public LogMessage(string module, LogMessageType type, string text, Exception exception, CallerInfo callerInfo)
        {
            Module = module;
            Type = type;
            Text = text;
            Exception = exception;
            CallerInfo = callerInfo;
        }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{(Module != null ? $"[{Module}]: " : string.Empty)}{Type}: {Text}{(Exception != null ? $". {Exception}" : string.Empty)}";
        }
    }
}
