// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Annotations;

namespace Stride.Core
{
    /// <summary>
    /// A service registry is a <see cref="IServiceProvider"/> that provides methods to register and unregister services.
    /// </summary>
    public interface IServiceRegistry
    {
        /// <summary>
        /// Occurs when a new service is added.
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceAdded;

        /// <summary>
        /// Occurs when when a service is removed.
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceRemoved;

        /// <summary>
        /// Adds a service to this <see cref="ServiceRegistry"/>.
        /// </summary>
        /// <typeparam name="T">The type of service to add.</typeparam>
        /// <param name="service">The service to add.</param>
        /// <exception cref="ArgumentNullException">Service cannot be null</exception>
        /// <exception cref="ArgumentException">Service is already registered with this type</exception>
        void AddService<T>([NotNull] T service) where T : class;

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>A service of the requested type, or [null] if not found.</returns>
        [CanBeNull]
        T GetService<T>() where T : class;

        /// <summary>
        /// Removes the object providing a specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        void RemoveService<T>() where T : class;
    }
}
