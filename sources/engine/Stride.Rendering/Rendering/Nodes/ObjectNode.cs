// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
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
