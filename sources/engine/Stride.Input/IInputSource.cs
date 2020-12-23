// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Collections;

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface for a platform specific mechanism that provides input in the form of one or
    ///   multiple <see cref="IInputDevice"/>s.
    /// </summary>
    /// <remarks>
    ///   An input source is responsible for cleaning up it's own devices at cleanup.
    /// </remarks>
    public interface IInputSource : IDisposable
    {
        /// <summary>
        ///   Gets all the input devices currently proviced by this source.
        /// </summary>
        TrackingDictionary<Guid, IInputDevice> Devices { get; }

        /// <summary>
        ///   Initializes the input source.
        /// </summary>
        /// <param name="inputManager">The <see cref="InputManager"/> initializing this source.</param>
        void Initialize(InputManager inputManager);

        /// <summary>
        ///   Allows the source to take it's time to search for new devices.
        /// </summary>
        void Scan();

        /// <summary>
        ///   Update the input source and possibly add/remove input devices.
        /// </summary>
        void Update();

        /// <summary>
        ///   Method called when input should be paused, for example when the application leaves the foreground.
        /// </summary>
        void Pause();

        /// <summary>
        ///   Method called when input should be resumed, for example when an application enters the foreground.
        /// </summary>
        void Resume();
    }
}
