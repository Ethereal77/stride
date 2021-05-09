// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Games
{
    /// <summary>
    ///   Defines the type of a <see cref="GameContext"/>.
    /// </summary>
    public enum AppContextType
    {
        /// <summary>
        ///   The game is running on desktop in a <see cref="System.Windows.Forms.Form"/> or <see cref="System.Windows.Forms.Control"/>.
        /// </summary>
        Desktop,

        /// <summary>
        ///   The game is running on desktop in a WPF <see cref="System.Windows.Window"/> through a <see cref="System.Windows.Interop.D3DImage"/>.
        /// </summary>
        DesktopWpf
    }
}
