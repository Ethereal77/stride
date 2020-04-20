// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Reflection;

namespace Stride.Games
{
    internal class GamePlatformWindows : GamePlatform
    {
        public GamePlatformWindows(GameBase game) : base(game)
        {
            IsBlockingRun = true;
#if STRIDE_RUNTIME_CORECLR
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
                 case AppContextType.Desktop:
#if STRIDE_GRAPHICS_API_DIRECT3D && STRIDE_UI_WINFORMS
                    return new GameWindowWinforms();
#else
                    return null;
#endif

#if STRIDE_UI_WPF
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
