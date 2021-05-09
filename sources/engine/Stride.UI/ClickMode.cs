// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI
{
    /// <summary>
    /// Specifies when the Click event should be raised.
    /// </summary>
    public enum ClickMode
    {
        /// <summary>
        /// Specifies that the Click event should be raised as soon as a button is pressed.
        /// </summary>
        /// <userdoc>Specifies that the Click event should be raised as soon as a button is pressed.</userdoc>
        Press,
        /// <summary>
        /// Specifies that the Click event should be raised when a button is pressed and released.
        /// </summary>
        /// <userdoc>Specifies that the Click event should be raised when a button is pressed and released.</userdoc>
        Release,
    }
}
