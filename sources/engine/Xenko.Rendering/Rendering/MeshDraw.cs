// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Graphics;

namespace Xenko.Rendering
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
