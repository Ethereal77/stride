// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents the base class for graphics resources.
    /// </summary>
    public partial class GraphicsResourceBase : ComponentBase
    {
        internal GraphicsResourceLifetimeState LifetimeState;

        public Action<GraphicsResourceBase> Reload;

        /// <summary>
        ///   Gets the graphics device attached to this instance.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        ///   Raised when the internal graphics resource gets destroyed.
        /// </summary>
        /// <remarks>
        ///   This event is useful when user allocated handles associated with the internal resource need to be released.
        /// </remarks>
        public event EventHandler<EventArgs> Destroyed;


        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsResourceBase"/> class.
        /// </summary>
        protected GraphicsResourceBase() : this(device: null, name: null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsResourceBase"/> class.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        protected GraphicsResourceBase(GraphicsDevice device) : this(device, name: null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsResourceBase"/> class.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="name">The name.</param>
        protected GraphicsResourceBase(GraphicsDevice device, string name) : base(name)
        {
            AttachToGraphicsDevice(device);
        }

        internal void AttachToGraphicsDevice(GraphicsDevice device)
        {
            GraphicsDevice = device;

            if (device != null)
            {
                // Add GraphicsResourceBase to device resources
                var resources = device.Resources;
                lock (resources)
                {
                    resources.Add(this);
                }
            }

            Initialize();
        }

        /// <summary>
        ///   Method called when the graphics device is inactive (put in the background, rendering is paused).
        ///   It should voluntarily release objects that can be easily recreated, such as render targets and
        ///   dynamic buffers.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the resource has transitioned to a <see cref="GraphicsResourceLifetimeState.Paused"/> state.
        /// </returns>
        protected internal virtual bool OnPause()
        {
            return false;
        }

        /// <summary>
        ///   Method called when the graphics device is resumed from either a paused or destroyed state.
        ///   If possible, the resource should be recreated.
        /// </summary>
        protected internal virtual void OnResume() { }

        /// <inheritdoc/>
        protected override void Destroy()
        {
            var device = GraphicsDevice;

            if (device != null)
            {
                // Remove GraphicsResourceBase from device resources
                var resources = device.Resources;
                lock (resources)
                {
                    resources.Remove(this);
                }
                if (LifetimeState != GraphicsResourceLifetimeState.Destroyed)
                {
                    OnDestroyed();
                    LifetimeState = GraphicsResourceLifetimeState.Destroyed;
                }
            }

            // No need for reload anymore, allow it to be GC
            Reload = null;

            base.Destroy();
        }
    }
}
