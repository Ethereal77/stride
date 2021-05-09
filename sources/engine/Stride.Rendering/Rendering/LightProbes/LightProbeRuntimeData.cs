// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

using Stride.Core;
using Stride.Core.Collections;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using Stride.Core.Serialization.Contents;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.ComputeEffect.LambertianPrefiltering;
using Stride.Rendering.Images.SphericalHarmonics;
using Stride.Rendering.LightProbes;
using Stride.Rendering.Skyboxes;

namespace Stride.Rendering.LightProbes
{
    public class LightProbeRuntimeData
    {
        /// <summary>
        /// Can be used to setup a link to a source.
        /// Typically, this might be a lightprobe component.
        /// </summary>
        public object[] LightProbes;

        // Computed data
        public Vector3[] Vertices;
        public int UserVertexCount;
        public FastList<BowyerWatsonTetrahedralization.Tetrahedron> Tetrahedra;
        public FastList<BowyerWatsonTetrahedralization.Face> Faces;

        // Data to upload to GPU
        public Color3[] Coefficients;
        public Vector4[] Matrices;
        public Int4[] LightProbeIndices;
    }
}
