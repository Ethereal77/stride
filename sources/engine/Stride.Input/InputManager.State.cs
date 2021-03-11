// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Stride.Core.Collections;
using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    ///   Represents a class that keeps track of the the global input state of all devices.
    /// </summary>
    public partial class InputManager :
        IInputEventListener<KeyEvent>,
        IInputEventListener<PointerEvent>,
        IInputEventListener<MouseWheelEvent>
    {
        private Vector2 mousePosition;
        private Vector2 absoluteMousePosition;

        private readonly ReadOnlySet<MouseButton> NoButtons = new ReadOnlySet<MouseButton>(new HashSet<MouseButton>());
        private readonly ReadOnlySet<Keys> NoKeys = new ReadOnlySet<Keys>(new HashSet<Keys>());

        private readonly List<KeyEvent> keyEvents = new List<KeyEvent>();

        private readonly List<PointerEvent> pointerEvents = new List<PointerEvent>();

        /// <summary>
        ///   Gets or sets the mouse position in normalized coordinates.
        /// </summary>
        public Vector2 MousePosition
        {
            get => mousePosition;
            set => SetMousePosition(value);
        }

        /// <summary>
        ///   Gets the mouse position in device coordinates.
        /// </summary>
        public Vector2 AbsoluteMousePosition => absoluteMousePosition;

        /// <summary>
        ///   Gets the mouse delta in normalized coordinates since the last frame.
        /// </summary>
        public Vector2 MouseDelta { get; private set; }

        /// <summary>
        ///   Gets the mouse delta in device coordinates since the last frame.
        /// </summary>
        public Vector2 AbsoluteMouseDelta { get; private set; }

        /// <summary>
        ///   Gets the delta value of the mouse wheel button since the last frame.
        /// </summary>
        public float MouseWheelDelta { get; private set; }

        /// <summary>
        ///   Gets the pointer device that is responsible for setting the current <see cref="MouseDelta"/> and <see cref="MousePosition"/>.
        /// </summary>
        public IPointerDevice LastPointerDevice { get; private set; }

        /// <summary>
        ///   Determines whether one or more keys are pressed.
        /// </summary>
        /// <returns><c>true</c> if one or more keys are pressed; otherwise, <c>false</c>.</returns>
        public bool HasPressedKeys
        {
            get
            {
                if (!HasKeyboard)
                    return false;

                return Keyboard.PressedKeys.Count > 0;
            }
        }

        /// <summary>
        ///   Determines whether one or more keys are released.
        /// </summary>
        /// <returns><c>true</c> if one or more keys are released; otherwise, <c>false</c>.</returns>
        public bool HasReleasedKeys
        {
            get
            {
                if (!HasKeyboard)
                    return false;

                return Keyboard.ReleasedKeys.Count > 0;
            }
        }

        /// <summary>
        ///   Determines whether one or more keys are currently down.
        /// </summary>
        /// <returns><c>true</c> if one or more keys are down; otherwise, <c>false</c>.</returns>
        public bool HasDownKeys
        {
            get
            {
                if (!HasKeyboard)
                    return false;

                return Keyboard.DownKeys.Count > 0;
            }
        }

        /// <summary>
        ///   Gets the keys that have been pressed since the last frame.
        /// </summary>
        public Core.Collections.IReadOnlySet<Keys> PressedKeys
        {
            get
            {
                if (!HasKeyboard)
                    return NoKeys;

                return Keyboard.PressedKeys;
            }
        }

        /// <summary>
        ///   Gets the keys that have been released since the last frame.
        /// </summary>
        public Core.Collections.IReadOnlySet<Keys> ReleasedKeys
        {
            get
            {
                if (!HasKeyboard)
                    return NoKeys;

                return Keyboard.ReleasedKeys;
            }
        }

        /// <summary>
        ///   Gets the keys that are currently down.
        /// </summary>
        public Core.Collections.IReadOnlySet<Keys> DownKeys
        {
            get
            {
                if (!HasKeyboard)
                    return NoKeys;

                return Keyboard.DownKeys;
            }
        }

        /// <summary>
        ///   Gets the mouse buttons that have been pressed since the last frame.
        /// </summary>
        public Core.Collections.IReadOnlySet<MouseButton> PressedButtons
        {
            get
            {
                if (!HasMouse)
                    return NoButtons;

                return Mouse.PressedButtons;
            }
        }

        /// <summary>
        ///   Gets the mouse buttons that have been released since the last frame.
        /// </summary>
        public Core.Collections.IReadOnlySet<MouseButton> ReleasedButtons
        {
            get
            {
                if (!HasMouse)
                    return NoButtons;

                return Mouse.ReleasedButtons;
            }
        }

        /// <summary>
        ///   Gets the mouse buttons that are currently down.
        /// </summary>
        public Core.Collections.IReadOnlySet<MouseButton> DownButtons
        {
            get
            {
                if (!HasMouse)
                    return NoButtons;

                return Mouse.DownButtons;
            }
        }

        /// <summary>
        ///   Determines whether one or more of the mouse buttons are pressed.
        /// </summary>
        /// <returns><c>true</c> if one or more of the mouse buttons are pressed; otherwise, <c>false</c>.</returns>
        public bool HasPressedMouseButtons
        {
            get
            {
                if (!HasMouse)
                    return false;

                return Mouse.PressedButtons.Count > 0;
            }
        }

        /// <summary>
        ///   Determines whether one or more of the mouse buttons are released.
        /// </summary>
        /// <returns><c>true</c> if one or more of the mouse buttons are released; otherwise, <c>false</c>.</returns>
        public bool HasReleasedMouseButtons
        {
            get
            {
                if (!HasMouse)
                    return false;

                return Mouse.ReleasedButtons.Count > 0;
            }
        }

        /// <summary>
        ///   Determines whether one or more of the mouse buttons are currently down.
        /// </summary>
        /// <returns><c>true</c> if one or more of the mouse buttons are down; otherwise, <c>false</c>.</returns>
        public bool HasDownMouseButtons
        {
            get
            {
                if (!HasMouse)
                    return false;

                return Mouse.DownButtons.Count > 0;
            }
        }

        /// <summary>
        ///   Gets the pointer events that have been registered since the last frame.
        /// </summary>
        public IReadOnlyList<PointerEvent> PointerEvents => pointerEvents;

        /// <summary>
        ///   Gets the key events that have been registered since the last frame.
        /// </summary>
        public IReadOnlyList<KeyEvent> KeyEvents => keyEvents;

        public void ProcessEvent(KeyEvent inputEvent)
        {
            keyEvents.Add(inputEvent);
        }

        public void ProcessEvent(PointerEvent inputEvent)
        {
            pointerEvents.Add(inputEvent);

            // Update position and delta from whatever device sends position updates
            LastPointerDevice = inputEvent.Pointer;

            if (inputEvent.Device is IMouseDevice)
            {
                mousePosition = inputEvent.Position;
                absoluteMousePosition = inputEvent.AbsolutePosition;

                // Add deltas together, so nothing gets lost if a down events gets sent after a move event with the actual delta
                MouseDelta += inputEvent.DeltaPosition;
                AbsoluteMouseDelta += inputEvent.AbsoluteDeltaPosition;
            }
        }

        public void ProcessEvent(MouseWheelEvent inputEvent)
        {
            if (Math.Abs(inputEvent.WheelDelta) > Math.Abs(MouseWheelDelta))
            {
                MouseWheelDelta = inputEvent.WheelDelta;
            }
        }

        /// <summary>
        ///   Determines whether the specified key has been pressed since the previous update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key is pressed; otherwise, <c>false</c>.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return Keyboard?.IsKeyPressed(key) ?? false;
        }

        /// <summary>
        ///   Determines whether the specified key has been released since the previous update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key is released; otherwise, <c>false</c>.</returns>
        public bool IsKeyReleased(Keys key)
        {
            return Keyboard?.IsKeyReleased(key) ?? false;
        }

        /// <summary>
        ///   Determines whether the specified key is being pressed down.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key is being pressed down; otherwise, <c>false</c>.</returns>
        public bool IsKeyDown(Keys key)
        {
            return Keyboard?.IsKeyDown(key) ?? false;
        }

        /// <summary>
        ///   Determines whether the specified mouse button has been pressed since the previous update.
        /// </summary>
        /// <param name="mouseButton">The mouse button.</param>
        /// <returns><c>true</c> if the specified mouse button is pressed since the previous update; otherwise, <c>false</c>.</returns>
        public bool IsMouseButtonPressed(MouseButton mouseButton)
        {
            return Mouse?.IsButtonPressed(mouseButton) ?? false;
        }

        /// <summary>
        ///   Determines whether the specified mouse button has been released since the previous update.
        /// </summary>
        /// <param name="mouseButton">The mouse button.</param>
        /// <returns><c>true</c> if the specified mouse button is released; otherwise, <c>false</c>.</returns>
        public bool IsMouseButtonReleased(MouseButton mouseButton)
        {
            return Mouse?.IsButtonReleased(mouseButton) ?? false;
        }

        /// <summary>
        ///   Determines whether the specified mouse button is being pressed down.
        /// </summary>
        /// <param name="mouseButton">The mouse button.</param>
        /// <returns><c>true</c> if the specified mouse button is being pressed down; otherwise, <c>false</c>.</returns>
        public bool IsMouseButtonDown(MouseButton mouseButton)
        {
            return Mouse?.IsButtonDown(mouseButton) ?? false;
        }

        /// <summary>
        ///   Resets the state before updating.
        /// </summary>
        private void ResetGlobalInputState()
        {
            keyEvents.Clear();
            pointerEvents.Clear();
            MouseWheelDelta = 0;
            MouseDelta = Vector2.Zero;
            AbsoluteMouseDelta = Vector2.Zero;
        }
    }
}
