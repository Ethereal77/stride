// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI
{
    /// <summary>
    /// Indicates where an element should be displayed on the horizontal axis relative to the allocated layout slot of the parent element.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// An element aligned to the left of the layout slot for the parent element.
        /// </summary>
        /// <userdoc>An element aligned to the left of the layout slot for the parent element.</userdoc>
        Left,
        /// <summary>
        /// An element aligned to the center of the layout slot for the parent element.
        /// </summary>
        /// <userdoc>An element aligned to the center of the layout slot for the parent element.</userdoc>
        Center,
        /// <summary>
        /// An element aligned to the right of the layout slot for the parent element.
        /// </summary>
        /// <userdoc>An element aligned to the right of the layout slot for the parent element.</userdoc>
        Right,
        /// <summary>
        /// An element stretched to fill the entire layout slot of the parent element.
        /// </summary>
        /// <userdoc>An element stretched to fill the entire layout slot of the parent element.</userdoc>
        Stretch,
    }
}
