// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
{
    /// <summary>
    /// Represents a <see cref="RenderObject"/> and allows to attach properties every frame.
    /// </summary>
    public struct ObjectNode
    {
        /// <summary>
        /// Access underlying RenderObject.
        /// </summary>
        public RenderObject RenderObject;

        public ObjectNode(RenderObject renderObject)
        {
            RenderObject = renderObject;
        }
    }
}
