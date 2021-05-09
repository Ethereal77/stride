// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents a resource that depends on the graphics device.
    /// </summary>
    public abstract partial class GraphicsResource : GraphicsResourceBase
    {
        protected GraphicsResource() { }

        protected GraphicsResource(GraphicsDevice device) : base(device) { }

        protected GraphicsResource(GraphicsDevice device, string name) : base(device, name) { }
    }
}
