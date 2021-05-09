// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.InteropServices;

using Stride.Core.Mathematics;

namespace Stride.Rendering.Shadows
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ShadowMapCascadeInfo
    {
        public ShadowMapCascadeLevel CascadeLevels;
        public Matrix ViewProjCaster;
        public Vector4 CascadeTextureCoords;
    }
}
