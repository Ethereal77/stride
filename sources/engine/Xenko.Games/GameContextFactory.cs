// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Games
{
    /// <summary>
    /// Given a <see cref="AppContextType"/> creates the corresponding GameContext instance based on the current executing platform.
    /// </summary>
    public static class GameContextFactory
    {
        [Obsolete("Use NewGameContext with the proper AppContextType.")]
        internal static GameContext NewDefaultGameContext()
        {
            // Default context is Desktop
            return NewGameContext(AppContextType.Desktop);
        }

        /// <summary>
        /// Given a <paramref name="type"/> create the appropriate game Context for the current executing platform.
        /// </summary>
        /// <returns></returns>
        public static GameContext NewGameContext(AppContextType type)
        {
            GameContext res = null;
            switch (type)
            {
                case AppContextType.Desktop:
                    res = NewGameContextDesktop();
                    break;
                case AppContextType.DesktopSDL:
                    res = NewGameContextSDL();
                    break;
                case AppContextType.DesktopWpf:
                    res = NewGameContextWpf();
                    break;
            }

            if (res == null)
            {
                throw new InvalidOperationException("Requested type and current platform are incompatible.");
            }

            return res;
        }

        public static GameContext NewGameContextDesktop()
        {
#if XENKO_UI_SDL && !XENKO_UI_WINFORMS && !XENKO_UI_WPF
        return new GameContextSDL(null);
#elif (XENKO_UI_WINFORMS || XENKO_UI_WPF)
        return new GameContextWinforms(null);
#else
        return null;
#endif
        }

        public static GameContext NewGameContextSDL()
        {
#if XENKO_UI_SDL
            return new GameContextSDL(null);
#else
            return null;
#endif
        }

        public static GameContext NewGameContextWpf()
        {
            // Not supported for now.
            return null;
        }
    }
}
