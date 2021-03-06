// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.TextureConverter.Requests
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
