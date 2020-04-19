// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Yaml
{
    /// <summary>
    /// Base exception that is thrown when the a problem occurs in the SharpYaml library.
    /// </summary>
    public class YamlException : Exception
    {
        /// <summary>
        /// Gets the position in the input stream where the event that originated the exception starts.
        /// </summary>
        public Mark Start { get; private set; }

        /// <summary>
        /// Gets the position in the input stream where the event that originated the exception ends.
        /// </summary>
        public Mark End { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public YamlException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException(Mark start, Mark end, string message)
            : this(start, end, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException(Mark start, Mark end, string message, Exception innerException)
            : base(string.Format("({0}) - ({1}): {2}", start, end, message), innerException)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public YamlException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}