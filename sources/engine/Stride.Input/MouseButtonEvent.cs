// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    /// Describes a button on a mouse changing state
    /// </summary>
    public class MouseButtonEvent : ButtonEvent
    {
        /// <summary>
        /// The button that changed state
        /// </summary>
        public MouseButton Button;

        /// <summary>
        /// The mouse that sent this event
        /// </summary>
        public IMouseDevice Mouse => (IMouseDevice)Device;

        public override string ToString()
        {
            return $"{nameof(Button)}: {Button}, {nameof(IsDown)}: {IsDown}, {nameof(Mouse)}: {Mouse.Name}";
        }
    }
}