// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Presentation.Windows
{
    /// <summary>
    /// An enum representing the initial position of a window shown with the <see cref="WindowManager"/>.
    /// </summary>
    public enum WindowInitialPosition
    {
        /// <summary>
        /// The window will be displayed centered relative to its owner.
        /// </summary>
        CenterOwner,
        /// <summary>
        /// The window will be displayed centered relative to the screen.
        /// </summary>
        CenterScreen,
        /// <summary>
        /// The window will be displayed close to the mouse cursor.
        /// </summary>
        MouseCursor,
    };
}
