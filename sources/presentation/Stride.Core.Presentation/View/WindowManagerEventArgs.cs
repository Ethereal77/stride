// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Presentation.Windows;

namespace Stride.Core.Presentation.View
{
    /// <summary>
    /// Arguments for events raised by the <see cref="WindowManager"/> class.
    /// </summary>
    public class WindowManagerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManagerEventArgs"/> class.
        /// </summary>
        /// <param name="window">The info of the window related to this event.</param>
        internal WindowManagerEventArgs(WindowInfo window)
        {
            Window = window;
        }

        /// <summary>
        /// Gets the info of the window related to this event.
        /// </summary>
        public WindowInfo Window { get; }
    }
}
