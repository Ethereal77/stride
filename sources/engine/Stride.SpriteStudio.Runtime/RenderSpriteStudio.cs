// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.SpriteStudio.Runtime
{
    public class RenderSpriteStudio : RenderObject
    {
        public Matrix WorldMatrix;

        public SpriteStudioSheet Sheet;
        public List<SpriteStudioNodeState> SortedNodes = new List<SpriteStudioNodeState>();
    }
}
