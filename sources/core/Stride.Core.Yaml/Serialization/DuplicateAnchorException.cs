// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// The exception that is thrown when a duplicate anchor is detected.
    /// </summary>
    public class DuplicateAnchorException : YamlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateAnchorException"/> class.
        /// </summary>
        public DuplicateAnchorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateAnchorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DuplicateAnchorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateAnchorException"/> class.
        /// </summary>
        public DuplicateAnchorException(Mark start, Mark end, string message)
            : base(start, end, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateAnchorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DuplicateAnchorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}