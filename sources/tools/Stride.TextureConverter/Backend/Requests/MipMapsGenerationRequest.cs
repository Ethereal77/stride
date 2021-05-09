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
    /// Request to generate the mipmap chain on a texture (3d texture mipmap generation not yet supported)
    /// </summary>
    internal class MipMapsGenerationRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.MipMapsGeneration; } }


        /// <summary>
        /// The filter to be used when rescaling to create the mipmaps of lower level.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public Filter.MipMapGeneration Filter { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MipMapsGenerationRequest"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public MipMapsGenerationRequest(Filter.MipMapGeneration filter)
        {
            Filter = filter;
        }
    }
}
