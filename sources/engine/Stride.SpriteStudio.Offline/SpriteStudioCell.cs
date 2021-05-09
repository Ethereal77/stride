// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Mathematics;

namespace Stride.SpriteStudio.Offline
{
    public class SpriteStudioAnim
    {
        public string Name;
        public int Fps;
        public int FrameCount;
        public Dictionary<string, NodeAnimationData> NodesData = new Dictionary<string, NodeAnimationData>();
    }

    public class SpriteStudioCell
    {
        public string Name;
        public RectangleF Rectangle;
        public Vector2 Pivot;
        public int TextureIndex;
    }
}
