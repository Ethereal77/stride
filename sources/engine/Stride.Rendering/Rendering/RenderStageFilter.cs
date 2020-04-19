// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Rendering
{
    /// <summary>
    /// Defines a way to filter RenderObject.
    /// </summary>
    [DataContract("RenderStageFilter")]
    public abstract class RenderStageFilter
    {
        public abstract bool IsVisible(RenderObject renderObject, RenderView renderView, RenderViewStage renderViewStage);
    }
}
