// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Yaml.Tokens
{
    /// <summary>
    /// Base class for YAML tokens.
    /// </summary>
    public abstract class Token
    {
        private readonly Mark start;

        /// <summary>
        /// Gets the start of the token in the input stream.
        /// </summary>
        public Mark Start { get { return start; } }

        private readonly Mark end;

        /// <summary>
        /// Gets the end of the token in the input stream.
        /// </summary>
        public Mark End { get { return end; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        protected Token(Mark start, Mark end)
        {
            this.start = start;
            this.end = end;
        }
    }
}