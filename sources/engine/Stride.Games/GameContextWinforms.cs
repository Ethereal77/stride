// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

using System;
using System.Windows.Forms;

namespace Stride.Games
{
    /// <summary>
    /// A <see cref="GameContext"/> to use for rendering to an existing WinForm <see cref="Control"/>.
    /// </summary>
    public class GameContextWinforms : GameContextWindows<Control>
    {
        /// <inheritDoc/>
        /// <param name="isUserManagingRun">Is user managing event processing of <paramref name="control"/>?</param>
        public GameContextWinforms(Control control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
            : base(control ?? CreateForm(), requestedWidth, requestedHeight)
        {
            ContextType = AppContextType.Desktop;
            IsUserManagingRun = isUserManagingRun;
        }

        /// <summary>
        /// Gets the run loop to be called when <see cref="IsUserManagingRun"/> is true.
        /// </summary>
        /// <value>The run loop.</value>
        public Action RunCallback { get; internal set; }

        /// <summary>
        /// Gets the exit callback to be called when <see cref="IsUserManagingRun"/> is true when exiting the game.
        /// </summary>
        /// <value>The run loop.</value>
        public Action ExitCallback { get; internal set; }

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
