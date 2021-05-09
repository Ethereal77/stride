// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.TextureConverter.Requests
{
    /// <summary>
    /// Request to create a texture array from a texture list.
    /// </summary>
    class ArrayCreationRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.ArrayCreation; } }

        /// <summary>
        /// The texture list that will populate the array.
        /// </summary>
        public List<TexImage> TextureList { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayCreationRequest"/> class.
        /// </summary>
        /// <param name="textureList">The texture list.</param>
        public ArrayCreationRequest(List<TexImage> textureList)
        {
            TextureList = textureList;
        }
    }
}
