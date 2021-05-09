// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Annotations;

namespace Stride.Physics
{
    [DataContract]
    public struct HeightfieldCenteringParameters
    {
        [DataMember(10)]
        public bool Enabled { get; set; }

        /// <summary>
        ///   The height to be centered.
        /// </summary>
        [DataMember(20)]
        [InlineProperty]
        public float CenterHeight { get; set; }

        public bool Match(HeightfieldCenteringParameters other)
        {
            return other.Enabled == Enabled &&
                   Math.Abs(other.CenterHeight - CenterHeight) < float.Epsilon;
        }
    }
}
