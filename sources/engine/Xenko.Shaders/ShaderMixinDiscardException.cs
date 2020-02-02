// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Shaders
{
    /// <summary>
    /// An exception used to early exit a shader mixin with discard.
    /// </summary>
    public class ShaderMixinDiscardException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderMixinDiscardException"/> class.
        /// </summary>
        public ShaderMixinDiscardException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderMixinDiscardException"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public ShaderMixinDiscardException(ShaderSource source)
        {
            DiscardSource = source;
        }

        /// <summary>
        /// Gets the discard source if any (may be null).
        /// </summary>
        /// <value>The discard source.</value>
        public ShaderSource DiscardSource { get; private set; }
    }
}
