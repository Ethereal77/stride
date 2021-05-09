// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

using System;
using System.Windows.Forms;

namespace Stride.Games
{
    /// <summary>
    ///   Represents a <see cref="GameContext"/> that can be used for rendering to an existing WinForms <see cref="Control"/>.
    /// </summary>
    public class GameContextWinforms : GameContext<Control>
    {
        /// <inheritDoc/>
        /// <param name="isUserManagingRun">A value indicating if the user will manage the event processing of <paramref name="control"/>.</param>
        public GameContextWinforms(Control control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
            : base(control ?? CreateForm(), requestedWidth, requestedHeight, isUserManagingRun)
        {
            ContextType = AppContextType.Desktop;
        }

        private static Form CreateForm()
        {
#if !STRIDE_GRAPHICS_API_NULL
            return new GameForm();
#else
            // Not Reachable.
            return null;
#endif
        }
    }
}

#endif
