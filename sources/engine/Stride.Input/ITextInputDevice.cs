// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Input
{
    /// <summary>
    /// A device such as a keyboard that supports text input. This can be a Windows keyboard with IME support or a touch keyboard on a mobile device.
    /// </summary>
    public interface ITextInputDevice : IInputDevice
    {
        /// <summary>
        /// Allows input to be entered, the input device will then send text events through the input manager
        /// </summary>
        void EnabledTextInput();

        /// <summary>
        /// Disallows text input to be entered, will close any IME active and stop sending text events
        /// </summary>
        void DisableTextInput();
    }
}
