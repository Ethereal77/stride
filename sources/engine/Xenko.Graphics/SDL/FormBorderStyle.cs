// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_UI_SDL

namespace Xenko.Graphics.SDL
{
    /// <summary>
    /// Set of border style to mimic the Windows forms one. We actually only show <see cref="FixedSingle"/> and <see cref="Sizable"/> as the
    /// other values don't make sense in a purely SDL context.
    /// </summary>
    public enum FormBorderStyle
    {
        None = 0,
        FixedSingle = 1,
        Sizable = 4,
    }
}
#endif
