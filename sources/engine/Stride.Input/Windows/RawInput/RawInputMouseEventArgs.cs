// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

namespace Stride.Input.RawInput
{
    internal struct RawInputMouseEventArgs
    {
        public bool isRelative { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}

#endif
