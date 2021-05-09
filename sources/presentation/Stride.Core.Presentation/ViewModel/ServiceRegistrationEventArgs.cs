// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Presentation.ViewModel
{
    /// <summary>
    /// Arguments of the events raised by <see cref="IViewModelServiceProvider"/> implementations.
    /// </summary>
    public class ServiceRegistrationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistrationEventArgs"/> class.
        /// </summary>
        /// <param name="service">The service related to this event.</param>
        internal ServiceRegistrationEventArgs(object service)
        {
            Service = service;
        }

        /// <summary>
        /// Gets the service related to this event.
        /// </summary>
        public object Service { get; }
    }
}
