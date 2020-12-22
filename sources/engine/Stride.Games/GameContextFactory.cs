// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Games
{
    /// <summary>
    ///   Provides methods to creates <see cref="GameContext"/> based on the provided <see cref="AppContextType"/> and the current executing platform.
    /// </summary>
    public static class GameContextFactory
    {
        [Obsolete("Use NewGameContext with the proper AppContextType.")]
        internal static GameContext NewDefaultGameContext(int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
        {
            // Default context is Desktop
            return NewGameContext(AppContextType.Desktop, requestedWidth, requestedHeight, isUserManagingRun);
        }

        /// <summary>
        ///   Creates the appropriate <see cref="GameContext"/> for the current executing platform and <see cref="AppContextType"/>.
        /// </summary>
        /// <returns>The created <see cref="GameContext"/>.</returns>
        public static GameContext NewGameContext(AppContextType type, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
        {
            switch (type)
            {
                case AppContextType.Desktop:
                    return NewGameContextDesktop(requestedWidth, requestedHeight, isUserManagingRun);

                case AppContextType.DesktopWpf:
                    return NewGameContextWpf(requestedWidth, requestedHeight, isUserManagingRun);

                default:
                    throw new InvalidOperationException("Requested type and current platform are incompatible.");
            }
        }

        public static GameContext NewGameContextDesktop(int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
        {
#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF
            return new GameContextWinforms(null, requestedWidth, requestedHeight, isUserManagingRun);
#else
            return null;
#endif
        }

        public static GameContext NewGameContextWpf(int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false)
        {
            // Not supported for now
            return null;
        }
    }
}
