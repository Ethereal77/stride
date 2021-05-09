// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

namespace Stride.Input.RawInput
{
    internal enum UsagePage : ushort
    {
        HID_USAGE_PAGE_GENERIC = 0x01,
        HID_USAGE_PAGE_GAME = 0x05,
        HID_USAGE_PAGE_LED = 0x08,
        HID_USAGE_PAGE_BUTTON = 0x09
    }
}

#endif
