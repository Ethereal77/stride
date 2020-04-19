// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_UI_WINFORMS || XENKO_UI_WPF

using SharpDX.DirectInput;

namespace Xenko.Input
{
    internal class DirectInputState : IDeviceState<RawJoystickState, JoystickUpdate>
    {
        public bool[] Buttons = new bool[128];
        public float[] Axes = new float[8];
        public int[] PovControllers = new int[4];

        public unsafe void MarshalFrom(ref RawJoystickState value)
        {
            fixed (int* axesPtr = &value.X)
            {
                for (int i = 0; i < 8; i++)
                    Axes[i] = axesPtr[i] / 65535.0f;
            }
            fixed (byte* buttonsPtr = value.Buttons)
            {
                for (int i = 0; i < Buttons.Length; i++)
                    Buttons[i] = buttonsPtr[i] != 0;
            }
            fixed (int* povPtr = value.PointOfViewControllers)
            {
                for (int i = 0; i < PovControllers.Length; i++)
                    PovControllers[i] = povPtr[i];
            }
        }

        public void Update(JoystickUpdate update)
        {
        }
    }
}
#endif
