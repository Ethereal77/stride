// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Engine;

namespace Stride.Rendering
{
    /// <summary>
    /// An <see cref="EntityProcessor"/> dedicated for rendering.
    /// </summary>
    /// Note that it might be instantiated multiple times in a given <see cref="SceneInstance"/>.
    public interface IEntityComponentRenderProcessor
    {
        VisibilityGroup VisibilityGroup { get; set; }
    }
}
