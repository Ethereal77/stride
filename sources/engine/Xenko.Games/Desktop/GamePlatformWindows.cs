// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Reflection;

namespace Xenko.Games
{
    internal class GamePlatformWindows : GamePlatform
    {
        public GamePlatformWindows(GameBase game) : base(game)
        {
            IsBlockingRun = true;
#if XENKO_RUNTIME_CORECLR
                // This is required by the Audio subsystem of SharpDX.
            Win32Native.CoInitialize(IntPtr.Zero);
#endif
        }

        public override string DefaultAppDirectory
        {
            get
            {
                var assemblyUri = new Uri(Assembly.GetEntryAssembly().CodeBase);
                return Path.GetDirectoryName(assemblyUri.LocalPath);
            }
        }

        internal override GameWindow GetSupportedGameWindow(AppContextType type)
        {
            switch (type)
            {
#if XENKO_UI_SDL
                 case AppContextType.DesktopSDL:
                    return new GameWindowSDL();
#endif

                 case AppContextType.Desktop:
#if XENKO_GRAPHICS_API_DIRECT3D && XENKO_UI_WINFORMS
                    return new GameWindowWinforms();
#elif XENKO_UI_SDL
                    return new GameWindowSDL();
#else
                    return null;
#endif

#if XENKO_UI_WPF
                 case AppContextType.DesktopWpf:
                    // WPF is not supported yet.
                    return null;
#endif

                 default:
                    return null;
            }
        }
    }
}
