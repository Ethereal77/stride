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
    /// Request to switch the R and B channels on a texture.
    /// </summary>
    internal class SwitchingBRChannelsRequest : IRequest
    {
        public override RequestType Type { get { return RequestType.SwitchingChannels; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchingBRChannelsRequest"/> class.
        /// </summary>
        public SwitchingBRChannelsRequest()
        {
        }
    }
}
