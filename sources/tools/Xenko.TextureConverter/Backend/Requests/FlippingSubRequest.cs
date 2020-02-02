// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.TextureConverter.Requests
{
    internal class FlippingSubRequest : FlippingRequest
    {
        public override RequestType Type { get { return RequestType.FlippingSub; } }

        /// <summary>
        /// The index of the sub-image to flip.
        /// </summary>
        public int SubImageIndex;

        public FlippingSubRequest(int index, Orientation orientation)
            : base(orientation)
        {
            SubImageIndex = index;
        }
    }
}
