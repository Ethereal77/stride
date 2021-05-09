// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    ///   Represents the base class and common functionality for mouse devices.
    /// </summary>
    public abstract class MouseDeviceBase : PointerDeviceBase, IMouseDevice
    {
        protected MouseDeviceState MouseState;

        protected MouseDeviceBase()
        {
            MouseState = new MouseDeviceState(PointerState, this);
        }

        public abstract bool IsPositionLocked { get; }

        public Core.Collections.IReadOnlySet<MouseButton> PressedButtons => MouseState.PressedButtons;
        public Core.Collections.IReadOnlySet<MouseButton> ReleasedButtons => MouseState.ReleasedButtons;
        public Core.Collections.IReadOnlySet<MouseButton> DownButtons => MouseState.DownButtons;

        public Vector2 Position => MouseState.Position;
        public Vector2 Delta => MouseState.Delta;

        public override void Update(List<InputEvent> inputEvents)
        {
            base.Update(inputEvents);
            MouseState.Update(inputEvents);
        }

        public abstract void SetPosition(Vector2 normalizedPosition);

        public abstract void LockPosition(bool forceCenter = false);

        public abstract void UnlockPosition();
    }
}
