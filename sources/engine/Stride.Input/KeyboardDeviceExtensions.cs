// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    public static class KeyboardDeviceExtensions
    {
        /// <summary>
        /// Determines whether the specified key is pressed since the previous update.
        /// </summary>
        /// <param name="keyboardDevice">The keyboard</param>
        /// <param name="key">The key</param>
        /// <returns><c>true</c> if the specified key is pressed; otherwise, <c>false</c>.</returns>
        public static bool IsKeyPressed(this IKeyboardDevice keyboardDevice, Keys key)
        {
            return keyboardDevice.PressedKeys.Contains(key);
        }

        /// <summary>
        /// Determines whether the specified key is released since the previous update.
        /// </summary>
        /// <param name="keyboardDevice">The keyboard</param>
        /// <param name="key">The key</param>
        /// <returns><c>true</c> if the specified key is released; otherwise, <c>false</c>.</returns>
        public static bool IsKeyReleased(this IKeyboardDevice keyboardDevice, Keys key)
        {
            return keyboardDevice.ReleasedKeys.Contains(key);
        }

        /// <summary>
        /// Determines whether the specified key is being pressed down
        /// </summary>
        /// <param name="keyboardDevice">The keyboard</param>
        /// <param name="key">The key</param>
        /// <returns><c>true</c> if the specified key is being pressed down; otherwise, <c>false</c>.</returns>
        public static bool IsKeyDown(this IKeyboardDevice keyboardDevice, Keys key)
        {
            return keyboardDevice.DownKeys.Contains(key);
        }
    }
}