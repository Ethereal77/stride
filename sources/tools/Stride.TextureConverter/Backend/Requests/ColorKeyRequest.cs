// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.TextureConverter.Requests
{
    /// <summary>
    /// Request to premultiply the alpha on the texture
    /// </summary>
    class ColorKeyRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.ColorKey; } }

        /// <summary>
        /// Gets or sets the color key.
        /// </summary>
        /// <value>The color key.</value>
        public Color ColorKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorKeyRequest"/> class.
        /// </summary>
        public ColorKeyRequest(Color colorKey)
        {
            ColorKey = colorKey;
        }
    }
}
