// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.Background
{
    public class RenderBackground : RenderObject
    {
        public bool Is2D;
        public Texture Texture;
        public float Intensity;
        public Quaternion Rotation;
    }
}
