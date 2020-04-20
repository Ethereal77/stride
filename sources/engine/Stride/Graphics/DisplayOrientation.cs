// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Describes the orientation of the display.
    /// </summary>
    [Flags]
    [DataContract("DisplayOrientation")]
    public enum DisplayOrientation
    {
        /// <summary>
        /// The default value for the orientation.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Displays in landscape mode to the left.
        /// </summary>
        LandscapeLeft = 1,

        /// <summary>
        /// Displays in landscape mode to the right.
        /// </summary>
        LandscapeRight = 2,

        /// <summary>
        /// Displays in portrait mode.
        /// </summary>
        Portrait = 4,
    }
}
