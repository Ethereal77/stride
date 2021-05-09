// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Mathematics;

namespace SpaceEscape.Background
{
    /// <summary>
    /// This class contains information needed to describe a hole in the background.
    /// </summary>
    [DataContract("BackgroundElement")]
    public class Hole
    {
        /// <summary>
        /// The area of the hole.
        /// </summary>
        public RectangleF Area { get; set; }

        /// <summary>
        /// The height of the hole.
        /// </summary>
        public float Height { get; set; }
    }
}
