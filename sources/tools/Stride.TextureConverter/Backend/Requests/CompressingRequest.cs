// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.TextureConverter.Requests
{
    /// <summary>
    /// Request to compress a texture to a specified format
    /// </summary>
    internal class CompressingRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.Compressing; } }


        /// <summary>
        /// The format.
        /// </summary>
        public Stride.Graphics.PixelFormat Format { get; private set; }

        /// <summary>
        /// Gets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public TextureQuality Quality { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressingRequest"/> class.
        /// </summary>
        /// <param name="format">The compression format.</param>
        public CompressingRequest(Stride.Graphics.PixelFormat format, TextureQuality quality = TextureQuality.Fast)
        {
            this.Format = format;
            this.Quality = quality;
        }
    }
}
