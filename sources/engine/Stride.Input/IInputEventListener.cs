// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Input
{
    /// <summary>
    ///   Defines a marker interface for a class that does not listen to any event but is used to pass around
    ///   a type that might potentially listen for input events.
    /// </summary>
    public interface IInputEventListener { }

    /// <summary>
    ///   Defines the interface for a class that wants to listen to input events of a certain type.
    /// </summary>
    /// <typeparam name="TEventType">The type of <see cref="InputEvent"/> that will be sent to this event listener.</typeparam>
    public interface IInputEventListener<TEventType> : IInputEventListener
        where TEventType : InputEvent
    {
        /// <summary>
        ///   Processes a new input event.
        /// </summary>
        /// <param name="inputEvent">The input event.</param>
        void ProcessEvent(TEventType inputEvent);
    }
}
