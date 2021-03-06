// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    /// <summary>
    /// Represents a <see cref="RenderObject"/> from a specific view.
    /// </summary>
    public struct ViewObjectNode
    {
        /// <summary>
        /// Access underlying RenderObject.
        /// </summary>
        public readonly RenderObject RenderObject;

        // TODO: This can properly be removed and stored as a RenderView, [RenderPerViewNode start..end]
        public readonly RenderView RenderView;

        /// <summary>
        /// The object node reference.
        /// </summary>
        public readonly ObjectNodeReference ObjectNode;

        public ViewObjectNode(RenderObject renderObject, RenderView renderView, ObjectNodeReference objectNode)
        {
            RenderObject = renderObject;
            RenderView = renderView;
            ObjectNode = objectNode;
        }
    }
}
