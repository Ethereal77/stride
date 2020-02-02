// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Graphics
{ 
    /// <summary>
    /// GraphicsResource abstract class
    /// </summary>
    public abstract partial class GraphicsResource : GraphicsResourceBase
    {
        protected GraphicsResource()
        {
        }

        protected GraphicsResource(GraphicsDevice device) : base(device)
        {
        }

        protected GraphicsResource(GraphicsDevice device, string name) : base(device, name)
        {
        }
    }
}
