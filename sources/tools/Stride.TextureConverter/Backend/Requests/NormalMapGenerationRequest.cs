// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stride.TextureConverter.Requests
{
    class NormalMapGenerationRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.NormalMapGeneration; } }


        public float Amplitude { get; private set; }
        public TexImage NormalMap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalMapGenerationRequest"/> class.
        /// </summary>
        public NormalMapGenerationRequest(float amplitude)
        {
            Amplitude = amplitude;
        }
    }
}
