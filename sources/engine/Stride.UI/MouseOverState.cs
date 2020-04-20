// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.UI
{
    /// <summary>
    /// Describe the possible states of the mouse over an UI element.
    /// </summary>
    public enum MouseOverState
    {
        /// <summary>
        /// The mouse is neither over the element nor one of its children.
        /// </summary>
        /// <userdoc>The mouse is neither over the element nor one of its children.</userdoc>
        MouseOverNone,
        /// <summary>
        /// The mouse is over one of children of the element.
        /// </summary>
        /// <userdoc>The mouse is over one of children of the element.</userdoc>
        MouseOverChild,
        /// <summary>
        /// The mouse is directly over the element.
        /// </summary>
        /// <userdoc>The mouse is directly over the element.</userdoc>
        MouseOverElement,
    }
}
