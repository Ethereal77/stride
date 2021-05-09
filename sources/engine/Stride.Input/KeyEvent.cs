// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    /// Event for a keyboard button changing state
    /// </summary>
    public class KeyEvent : ButtonEvent
    {
        /// <summary>
        /// The key that is being pressed or released.
        /// </summary>
        public Keys Key;

        /// <summary>
        /// The repeat count for this key. If it is 0 this is the initial press of the key
        /// </summary>
        public int RepeatCount;

        /// <summary>
        /// The keyboard that sent this event
        /// </summary>
        public IKeyboardDevice Keyboard => (IKeyboardDevice)Device;

        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}, {nameof(IsDown)}: {IsDown}, {nameof(RepeatCount)}: {RepeatCount}, {nameof(Keyboard)}: {Keyboard}";
        }
    }
}