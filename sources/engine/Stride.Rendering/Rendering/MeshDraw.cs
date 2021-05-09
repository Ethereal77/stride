// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Graphics;

namespace Stride.Rendering
{
    // Need to add support for fields in auto data converter
    [DataContract]
    public class MeshDraw
    {
        public PrimitiveType PrimitiveType;

        public int DrawCount;

        public int StartLocation;

        public VertexBufferBinding[] VertexBuffers;

        public IndexBufferBinding IndexBuffer;
    }
}
