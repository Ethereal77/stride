// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// A default implementation of <see cref="IGraphicsDeviceService"/>
    /// </summary>
    public class GraphicsDeviceServiceLocal : IGraphicsDeviceService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDeviceServiceLocal"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public GraphicsDeviceServiceLocal(GraphicsDevice graphicsDevice) : this(null, graphicsDevice)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDeviceServiceLocal"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        public GraphicsDeviceServiceLocal(IServiceRegistry registry, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        // We provide an empty `add' and `remove' to avoid a warning about unused events that we have
        // to implement as they are part of the IGraphicsDeviceService definition.
        public event EventHandler<EventArgs> DeviceCreated { add { } remove { } }
        public event EventHandler<EventArgs> DeviceDisposing { add { } remove { } }
        public event EventHandler<EventArgs> DeviceReset { add { } remove { } }
        public event EventHandler<EventArgs> DeviceResetting { add { } remove { } }

        public GraphicsDevice GraphicsDevice { get; private set; }
    }
}
