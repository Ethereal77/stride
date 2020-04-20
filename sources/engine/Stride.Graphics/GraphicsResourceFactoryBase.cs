// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Base factory for all Graphics resources.
    /// </summary>
    public class GraphicsResourceFactoryBase : ComponentBase
    {
        protected internal GraphicsDevice GraphicsDevice { get; set; }

        protected internal GraphicsResourceFactoryBase(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }
    }
}
