// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Yaml.Tokens
{
    /// <summary>
    /// Represents a flow sequence end token.
    /// </summary>
    public class FlowSequenceEnd : Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlowSequenceEnd"/> class.
        /// </summary>
        public FlowSequenceEnd()
            : this(Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowSequenceEnd"/> class.
        /// </summary>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        public FlowSequenceEnd(Mark start, Mark end)
            : base(start, end)
        {
        }
    }
}