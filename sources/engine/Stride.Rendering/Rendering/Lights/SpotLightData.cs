﻿// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Rendering.Lights
{
    public struct SpotLightData
    {
        public Vector3 PositionWS;
        private float padding0;
        public Vector3 DirectionWS;
        private float padding1;
        public Vector3 AngleOffsetAndInvSquareRadius;
        private float padding2;
        public Color3 Color;
        private float padding3;
    }
}
