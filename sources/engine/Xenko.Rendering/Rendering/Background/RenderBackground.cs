// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace Xenko.Rendering.Background
{
    public class RenderBackground : RenderObject
    {
        public bool Is2D;
        public Texture Texture;
        public float Intensity;
        public Quaternion Rotation;
    }
}
