// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Games
{
    /// <summary>
    /// Type of a <see cref="GameContext"/>.
    /// </summary>
    public enum AppContextType
    {
        /// <summary>
        /// Game running on desktop in a form or <see cref="System.Windows.Forms.Control"/>.
        /// </summary>
        Desktop,

        /// <summary>
        /// Game running on desktop in a SDL window.
        /// </summary>
        DesktopSDL,

        /// <summary>
        /// Game running on desktop in a WPF window through a D3DImage.
        /// </summary>
        DesktopWpf
    }
}
