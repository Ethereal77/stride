// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.UI
{
    /// <summary>
    /// Describes the action of a specific touch point.
    /// </summary>
    public enum TouchAction
    {
        /// <summary>
        /// The act of putting a finger onto the screen.
        /// </summary>
        /// <userdoc>The act of putting a finger onto the screen.</userdoc>
        Down,
        /// <summary>
        /// The act of dragging a finger across the screen.
        /// </summary>
        /// <userdoc>The act of dragging a finger across the screen.</userdoc>
        Move,
        /// <summary>
        /// The act of lifting a finger off of the screen.
        /// </summary>
        /// <userdoc>The act of lifting a finger off of the screen.</userdoc>
        Up,
    }
}
