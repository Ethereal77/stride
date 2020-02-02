// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenko.TextureConverter.Requests
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
