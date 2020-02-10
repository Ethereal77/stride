// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Rendering;

namespace Xenko.SpriteStudio.Runtime
{
    public class RenderSpriteStudio : RenderObject
    {
        public Matrix WorldMatrix;

        public SpriteStudioSheet Sheet;
        public List<SpriteStudioNodeState> SortedNodes = new List<SpriteStudioNodeState>();
    }
}
