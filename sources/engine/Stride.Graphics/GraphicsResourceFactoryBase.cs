// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    ///   Base factory for all graphics resources.
    /// </summary>
    public class GraphicsResourceFactoryBase : ComponentBase
    {
        /// <summary>
        ///   The graphics device this factory is dependent on.
        /// </summary>
        protected internal GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsResourceFactoryBase"/> class.
        /// </summary>
        /// <param name="device">The graphics device this factory is dependent on.</param>
        protected internal GraphicsResourceFactoryBase(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }
    }
}
