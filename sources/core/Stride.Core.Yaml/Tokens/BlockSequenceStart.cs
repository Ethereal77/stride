// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml.Tokens
{
    /// <summary>
    /// Represents a block sequence start token.
    /// </summary>
    public class BlockSequenceStart : Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceStart"/> class.
        /// </summary>
        public BlockSequenceStart()
            : this(Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceStart"/> class.
        /// </summary>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        public BlockSequenceStart(Mark start, Mark end)
            : base(start, end)
        {
        }
    }
}