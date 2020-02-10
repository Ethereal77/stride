// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_UI_WINFORMS || XENKO_UI_WPF

using System;

using SharpDX.DirectInput;

namespace Xenko.Input
{
    internal class DirectInputJoystick : CustomDevice<DirectInputState, RawJoystickState, JoystickUpdate>
    {
        public DirectInputJoystick(IntPtr nativePtr) : base(nativePtr)
        {
        }

        public DirectInputJoystick(DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {
        }
    }
}
#endif