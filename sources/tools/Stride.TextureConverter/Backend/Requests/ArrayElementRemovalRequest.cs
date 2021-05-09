// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.TextureConverter.Requests
{
    /// <summary>
    /// Request to remove the texture at a specified position from a texture array.
    /// </summary>
    class ArrayElementRemovalRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.ArrayElementRemoval; } }

        /// <summary>
        /// The indice of the texture to be removed.
        /// </summary>
        public int Indice { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayElementRemovalRequest"/> class.
        /// </summary>
        /// <param name="indice">The indice of the texture to be removed.</param>
        public ArrayElementRemovalRequest(int indice)
        {
            Indice = indice;
        }
    }
}
