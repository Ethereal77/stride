// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.UI
{
    /// <summary>
    /// Specifies the display state of an element.
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// Display the element.
        /// </summary>
        /// <userdoc>Display the element.</userdoc>
        Visible,
        /// <summary>
        /// Do not display the element, but reserve space for the element in layout.
        /// </summary>
        /// <userdoc>Do not display the element, but reserve space for the element in layout.</userdoc>
        Hidden,
        /// <summary>
        /// Do not display the element, and do not reserve space for it in layout.
        /// </summary>
        /// <userdoc>Do not display the element, and do not reserve space for it in layout.</userdoc>
        Collapsed,
    }
}
