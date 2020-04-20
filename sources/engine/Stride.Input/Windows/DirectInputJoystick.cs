// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

using System;

using SharpDX.DirectInput;

namespace Stride.Input
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