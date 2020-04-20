// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml.Tokens
{
    /// <summary>
    /// Represents a scalar token.
    /// </summary>
    public class Scalar : Token
    {
        private readonly string value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get { return value; } }

        private readonly ScalarStyle style;

        /// <summary>
        /// Gets the style.
        /// </summary>
        /// <value>The style.</value>
        public ScalarStyle Style { get { return style; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scalar"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Scalar(string value)
            : this(value, ScalarStyle.Any)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scalar"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        public Scalar(string value, ScalarStyle style)
            : this(value, style, Mark.Empty, Mark.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scalar"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="style">The style.</param>
        /// <param name="start">The start position of the token.</param>
        /// <param name="end">The end position of the token.</param>
        public Scalar(string value, ScalarStyle style, Mark start, Mark end)
            : base(start, end)
        {
            this.value = value;
            this.style = style;
        }
    }
}