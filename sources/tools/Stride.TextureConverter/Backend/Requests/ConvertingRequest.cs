// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.TextureConverter.Requests
{
    /// <summary>
    /// Request to convert a texture to the specified format.
    /// </summary>
    internal class ConvertingRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.Converting; } }


        /// <summary>
        /// The destination format.
        /// </summary>
        public Stride.Graphics.PixelFormat Format { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertingRequest"/> class.
        /// </summary>
        /// <param name="format">The destination format.</param>
        public ConvertingRequest(Stride.Graphics.PixelFormat format)
        {
            this.Format = format;
        }
    }
}
