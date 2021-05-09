// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Shaders.Grammar
{
    /// <summary>
    /// Key terminal
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenInfo"/> class.
        /// </summary>
        public TokenInfo()
        {
            IsCaseInsensitive = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenInfo"/> class.
        /// </summary>
        /// <param name="tokenCategory">The token category.</param>
        public TokenInfo(TokenCategory tokenCategory)
        {
            TokenCategory = tokenCategory;
            IsCaseInsensitive = false;
        }

        /// <summary>
        /// Gets or sets the token category.
        /// </summary>
        /// <value>
        /// The token category.
        /// </value>
        public TokenCategory TokenCategory { get; set; }

        /// <summary>
        /// Gets or sets if this token is case insensitive (default false).
        /// </summary>
        public bool IsCaseInsensitive { get; set; }

    }
}

