// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI
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
