// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// Exception that is thrown when a semantic error is detected on a YAML stream.
    /// </summary>
    public class SemanticErrorException : YamlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticErrorException"/> class.
        /// </summary>
        public SemanticErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SemanticErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticErrorException"/> class.
        /// </summary>
        public SemanticErrorException(Mark start, Mark end, string message)
            : base(start, end, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public SemanticErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}