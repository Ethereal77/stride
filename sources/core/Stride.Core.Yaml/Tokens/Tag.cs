// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml.Tokens
{
    /// <summary>
    /// Represents a tag token.
    /// </summary>
    public class Tag : Token
    {
        private readonly string handle;
        private readonly string suffix;

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>The handle.</value>
        public string Handle { get { return handle; } }

        /// <summary>
        /// Gets the suffix.
        /// </summary>
        /// <value>The suffix.</value>
        public string Suffix { get { return suffix; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="suffix">The suffix.</param>
        public Tag(string handle, string suffix)
            : this(handle, suffix, Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        public Tag(string handle, string suffix, Mark start, Mark end)
            : base(start, end)
        {
            this.handle = handle;
            this.suffix = suffix;
        }
    }
}