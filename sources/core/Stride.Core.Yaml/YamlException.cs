// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml
{
    /// <summary>
    ///   Base exception that is thrown when the a problem occurs in the parsing of Yaml.
    /// </summary>
    public class YamlException : Exception
    {
        /// <summary>
        ///   Gets the position in the input stream where the event that originated the exception starts.
        /// </summary>
        public Mark Start { get; private set; }

        /// <summary>
        ///   Gets the position in the input stream where the event that originated the exception ends.
        /// </summary>
        public Mark End { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException(string message = null, Exception inner = null)
            : base(message, inner)
        { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException(Mark start, Mark end, string message, Exception innerException = null)
            : base($"{message} (({start}) -> ({end}))", innerException)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException(Events.ParsingEvent node, Exception innerException) :
            this(node.Start, node.End, $"An exception occured while deserializing node [{node}].", innerException)
        { }
    }
}
