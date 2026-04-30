// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Rendering.Lights
{
    public struct DirectionalLightData
    {
#pragma warning disable 169 // The field <X> is never used
        public Vector3 DirectionWS;
        private float padding0;
        public Color3 Color;
        private float padding1;
#pragma warning restore 169
    }
}
