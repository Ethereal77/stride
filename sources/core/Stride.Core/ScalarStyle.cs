// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core
{
    /// <summary>
    /// Specifies the style used for textual serialization of scalars.
    /// </summary>
    public enum ScalarStyle
    {
        /// <summary>
        /// Let the emitter choose the style.
        /// </summary>
        Any,

        /// <summary>
        /// The plain scalar style.
        /// </summary>
        Plain,

        /// <summary>
        /// The single-quoted scalar style.
        /// </summary>
        SingleQuoted,

        /// <summary>
        /// The double-quoted scalar style.
        /// </summary>
        DoubleQuoted,

        /// <summary>
        /// The literal scalar style.
        /// </summary>
        Literal,

        /// <summary>
        /// The folded scalar style.
        /// </summary>
        Folded,
    }
}
