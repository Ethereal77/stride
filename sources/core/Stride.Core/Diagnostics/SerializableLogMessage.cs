// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Represents a copy of a <see cref="LogMessage"/> that can be serialized.
    /// </summary>
    [DataContract, Serializable]
    public class SerializableLogMessage : ILogMessage
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
        ///   Gets or sets the exception info, in case this message represents an error or exception.
        /// </summary>
        public ExceptionInfo ExceptionInfo { get; set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref="SerializableLogMessage"/> class with default values for its properties.
        /// </summary>
        public SerializableLogMessage() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SerializableLogMessage"/> class from a <see cref="LogMessage"/> instance.
        /// </summary>
        /// <param name="message">The <see cref="LogMessage"/> instance to use to initialize properties.</param>
        public SerializableLogMessage([NotNull] LogMessage message)
        {
            Module = message.Module;
            Type = message.Type;
            Text = message.Text;
            ExceptionInfo = message.Exception != null ? new ExceptionInfo(message.Exception) : null;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SerializableLogMessage"/> class using the given parameters to set its properties.
        /// </summary>
        /// <param name="module">The module name.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <param name="exceptionInfo">The exception information. This parameter can be <c>null</c>.</param>
        public SerializableLogMessage([NotNull] string module, LogMessageType type, [NotNull] string text, ExceptionInfo exceptionInfo = null)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            Module = module;
            Type = type;
            Text = text;
            ExceptionInfo = exceptionInfo;
        }


        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{(Module != null ? $"[{Module}]: " : string.Empty)}{Type}: {Text}{(ExceptionInfo != null ? $". {ExceptionInfo.Message}" : string.Empty)}";
        }
    }
}
