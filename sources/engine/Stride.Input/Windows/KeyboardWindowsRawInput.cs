// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_INPUT_RAWINPUT

using System;

namespace Stride.Input
{
    internal class KeyboardWindowsRawInput : KeyboardDeviceBase
    {
        public KeyboardWindowsRawInput(InputSourceWindowsRawInput source)
        {
            // Raw input is usually preferred above other keyboards
            Priority = 100;
            Source = source;
        }

        public override string Name => "Windows Keyboard (Raw Input)";

        public override Guid Id => new Guid("d7437ff5-d14f-4491-9673-377b6d0e241c");

        public override IInputSource Source { get; }
    }
}
#endif