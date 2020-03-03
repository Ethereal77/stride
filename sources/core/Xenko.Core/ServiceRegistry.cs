// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Xenko.Core
{
    /// <summary>
    /// Base implementation for <see cref="IServiceRegistry"/>
    /// </summary>
    public class ServiceRegistry : IServiceRegistry
    {
        public static readonly PropertyKey<IServiceRegistry> ServiceRegistryKey = new PropertyKey<IServiceRegistry>(nameof(ServiceRegistryKey), typeof(IServiceRegistry));

        private readonly Dictionary<Type, object> registeredService = new Dictionary<Type, object>();

        /// <inheritdoc />
        public event EventHandler<ServiceEventArgs> ServiceAdded;

        /// <inheritdoc />
        public event EventHandler<ServiceEventArgs> ServiceRemoved;

        /// <inheritdoc />
        public T GetService<T>()
            where T : class
        {
            var type = typeof(T);
            lock (registeredService)
            {
                if (registeredService.TryGetValue(type, out var service))
                    return (T)service;
            }

            return null;
        }

        /// <inheritdoc />
        public void AddService<T>(T service)
            where T : class
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var type = typeof(T);
            lock (registeredService)
            {
                if (registeredService.ContainsKey(type))
                    throw new ArgumentException("Service is already registered with this type", nameof(type));
                registeredService.Add(type, service);
            }
            OnServiceAdded(new ServiceEventArgs(type, service));
        }

        /// <inheritdoc />
        public void RemoveService<T>()
            where T : class
        {
            var type = typeof(T);
            object oldService;
            lock (registeredService)
            {
                if (registeredService.TryGetValue(type, out oldService))
                    registeredService.Remove(type);
            }
            if (oldService != null)
                OnServiceRemoved(new ServiceEventArgs(type, oldService));
        }

        private void OnServiceAdded(ServiceEventArgs e)
        {
            ServiceAdded?.Invoke(this, e);
        }

        private void OnServiceRemoved(ServiceEventArgs e)
        {
            ServiceRemoved?.Invoke(this, e);
        }
    }
}
