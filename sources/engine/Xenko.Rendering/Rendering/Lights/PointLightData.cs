// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Mathematics;

namespace Xenko.Rendering.Lights
{
    public struct PointLightData
    {
        public Vector3 PositionWS;
        public float InvSquareRadius;
        public Color3 Color;
        private float padding0;
    }
}
