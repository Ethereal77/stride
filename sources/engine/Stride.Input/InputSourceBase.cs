// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Collections;

namespace Stride.Input
{
    /// <summary>
    ///   Represents the base class for input sources, implementing common parts of the <see cref="IInputSource"/>
    ///   interface and keeping track of registered devices.
    /// </summary>
    /// <remarks>
    ///   An input source is responsible for cleaning up it's own devices at cleanup.
    /// </remarks>
    public abstract class InputSourceBase : IInputSource
    {
        public TrackingDictionary<Guid, IInputDevice> Devices { get; } = new TrackingDictionary<Guid, IInputDevice>();

        public abstract void Initialize(InputManager inputManager);

        public virtual void Update() { }

        public virtual void Pause() { }

        public virtual void Resume() { }

        public virtual void Scan() { }

        /// <summary>
        ///   Unregisters all the input devices registered with <see cref="RegisterDevice"/> which have not been unregistered yet.
        /// </summary>
        public virtual void Dispose()
        {
            // Remove all devices, done by clearing the tracking dictionary
            Devices.Clear();
        }

        /// <summary>
        ///   Adds an input device.
        /// </summary>
        /// <param name="device">The device to register.</param>
        protected void RegisterDevice(IInputDevice device)
        {
            if (Devices.ContainsKey(device.Id))
                throw new InvalidOperationException($"Input device with Id {device.Id} already registered.");

            Devices.Add(device.Id, device);
        }

        /// <summary>
        ///   Removes a registered input device.
        /// </summary>
        /// <param name="device">The device to remove.</param>
        protected void UnregisterDevice(IInputDevice device)
        {
            if (!Devices.ContainsKey(device.Id))
                throw new InvalidOperationException($"Input device with Id {device.Id} was not registered.");

            Devices.Remove(device.Id);
        }
    }
}
