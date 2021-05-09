// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.InteropServices;

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.LightProbes
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LightProbeVertex
    {
        /// <summary>
        /// Initializes a new <see cref="LightProbeVertex"/> instance.
        /// </summary>
        /// <param name="position">The position of this vertex.</param>
        /// <param name="lightprobeId">The lightprobe ID.</param>
        public LightProbeVertex(Vector3 position, uint lightprobeId)
            : this()
        {
            Position = position;
            LightProbeId = lightprobeId;
        }

        /// <summary>
        /// XYZ position.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// The lightprobe index.
        /// </summary>
        public uint LightProbeId;

        /// <summary>
        /// Defines structure byte size.
        /// </summary>
        public static readonly int Size = 16;

        /// <summary>
        /// The vertex layout of this struct.
        /// </summary>
        public static readonly VertexDeclaration Layout = new VertexDeclaration(VertexElement.Position<Vector3>(), new VertexElement("LIGHTPROBE_ID", PixelFormat.R32_UInt));
    }
}
