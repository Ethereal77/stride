// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_UI_SDL

using Xenko.Graphics.SDL;

namespace Xenko.Games
{
    /// <summary>
    /// A <see cref="GameContext"/> to use for rendering to an existing SDL Window.
    /// </summary>
    public class GameContextSDL : GameContextWindows<Window>
    {
        static GameContextSDL()
        {
            // Preload proper SDL native library (depending on CPU type)
            Core.NativeLibrary.PreloadLibrary("SDL2.dll", typeof(Window));
        }

        /// <inheritDoc/>
        public GameContextSDL(Window control, int requestedWidth = 0, int requestedHeight = 0)
            : base(control ?? new GameFormSDL(), requestedWidth, requestedHeight) 
        {
            ContextType = AppContextType.DesktopSDL;
        }
    }
}
#endif
