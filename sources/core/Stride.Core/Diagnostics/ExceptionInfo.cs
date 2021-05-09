// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using System.Text;

using Stride.Core.Annotations;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Contains information and some properties of an <see cref="Exception"/>.
    /// </summary>
    [DataContract, Serializable]
    public sealed class ExceptionInfo
    {
        private static readonly ExceptionInfo[] EmptyExceptions = Array.Empty<ExceptionInfo>();

        /// <summary>
        ///   Gets or sets the message of the exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///   Gets or sets the stack trace of the exception.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        ///   Gets or sets the name of the exception type. Should correspond to the <see cref="Type.Name"/> property of the exception type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///   Gets or sets the full name of the exception type. Should correspond to the <see cref="Type.FullName"/> property of the exception type.
        /// </summary>
        public string TypeFullName { get; set; }

        /// <summary>
        ///   Gets or sets a collection of <see cref="ExceptionInfo"/> for the inner exceptions, if any.
        /// </summary>
        public ExceptionInfo[] InnerExceptions { get; set; } = EmptyExceptions;


        /// <summary>
        ///   Initializes a new instance of the <see cref="ExceptionInfo"/> class.
        /// </summary>
        public ExceptionInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfo"/> class from an <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">The exception used to initialize the properties of this instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is a <c>null</c> reference.</exception>
        public ExceptionInfo([NotNull] Exception exception)
        {
            if (exception is null)
                throw new ArgumentNullException(nameof(exception));

            Message = exception.Message;
            StackTrace = exception.StackTrace;
            TypeFullName = exception.GetType().FullName;
            TypeName = exception.GetType().Name;

            if (exception.InnerException != null)
            {
                InnerExceptions = new ExceptionInfo[] { new ExceptionInfo(exception.InnerException) };
            }
            else if (exception is ReflectionTypeLoadException reflectionException)
            {
                InnerExceptions = reflectionException.LoaderExceptions.Select(x => new ExceptionInfo(x)).ToArray();
            }
        }


        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(TypeName))
            {
                sb.Append(TypeName);
            }

            if (!string.IsNullOrEmpty(Message))
            {
                if(sb.Length != 0)
                    sb.Append(": ");
                sb.Append(Message);
            }

            foreach(var child in InnerExceptions)
            {
                sb.AppendFormat("{0} ---> {1}", Environment.NewLine, child.ToString());
            }

            if (StackTrace != null)
            {
                sb.AppendLine();
                sb.Append(StackTrace);
            }

            return sb.ToString();
        }
    }
}
