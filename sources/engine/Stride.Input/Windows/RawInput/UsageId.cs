// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

namespace Stride.Input.RawInput
{
    internal enum UsageId : ushort
    {
        HID_USAGE_GENERIC_POINTER = 0x01,
        HID_USAGE_GENERIC_MOUSE = 0x02,
        HID_USAGE_GENERIC_JOYSTICK = 0x04,
        HID_USAGE_GENERIC_GAMEPAD = 0x05,
        HID_USAGE_GENERIC_KEYBOARD = 0x06,
        HID_USAGE_GENERIC_KEYPAD = 0x07,
        HID_USAGE_GENERIC_MULTI_AXIS_CONTROLLER = 0x08,
    }
}

#endif
