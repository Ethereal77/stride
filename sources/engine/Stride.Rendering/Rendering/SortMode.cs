// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Rendering
{
    /// <summary>
    /// Defines a way to sort RenderObject.
    /// </summary>
    [DataContract("SortMode")]
    public abstract class SortMode
    {
        public abstract unsafe void GenerateSortKey(RenderView renderView, RenderViewStage renderViewStage, SortKey* sortKeys);
    }
}
