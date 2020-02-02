// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Engine;

namespace Xenko.Rendering
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
