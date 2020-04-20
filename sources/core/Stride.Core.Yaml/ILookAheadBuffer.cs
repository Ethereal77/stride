// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml
{
    internal interface ILookAheadBuffer
    {
        /// <summary>
        /// Gets a value indicating whether the end of the input reader has been reached.
        /// </summary>
        bool EndOfInput { get; }

        /// <summary>
        /// Gets the character at thhe specified offset.
        /// </summary>
        char Peek(int offset);

        /// <summary>
        /// Skips the next <paramref name="length"/> characters. Those characters must have been
        /// obtained first by calling the <see cref="Peek"/> method.
        /// </summary>
        void Skip(int length);
    }
}