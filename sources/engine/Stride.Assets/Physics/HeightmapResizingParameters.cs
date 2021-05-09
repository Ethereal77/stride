// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Core.Annotations;

namespace Stride.Assets.Physics
{
    [DataContract]
    public struct HeightmapResizingParameters
    {
        [DataMember(0)]
        public bool Enabled { get; set; }

        /// <summary>
        ///   New size of the heightmap.
        /// </summary>
        [DataMember(10)]
        [InlineProperty]
        public Int2 Size { get; set; }
    }
}
