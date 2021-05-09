// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Yaml.Tokens
{
    /// <summary>
    /// Represents a block entry event.
    /// </summary>
    public class BlockEntry : Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockEntry"/> class.
        /// </summary>
        public BlockEntry()
            : this(Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockEntry"/> class.
        /// </summary>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        public BlockEntry(Mark start, Mark end)
            : base(start, end)
        {
        }
    }
}