// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Input
{
    public class KeyboardSimulated : KeyboardDeviceBase
    {
        public KeyboardSimulated(InputSourceSimulated source)
        {
            Priority = -1000;
            Source = source;

            Id = Guid.NewGuid();
        }

        public override string Name => "Simulated Keyboard";

        public override Guid Id { get; }

        public override IInputSource Source { get; }

        public void SimulateDown(Keys key)
        {
            HandleKeyDown(key);
        }

        public void SimulateUp(Keys key)
        {
            HandleKeyUp(key);
        }
    }
}
