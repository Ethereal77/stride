// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Graphics
{
    /// <summary>
    /// Root factory for all Graphics components.
    /// </summary>
    public class GraphicsFactory : ComponentBase
    {
        /// <summary>
        /// GraphicsApi key.
        /// TODO not the best place to store this identifier. Move it to GraphicsFactory?
        /// </summary>
        public const string GraphicsApi = "GraphicsApi";
    }
}
