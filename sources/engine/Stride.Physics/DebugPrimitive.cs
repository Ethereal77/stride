// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

using Stride.Rendering;

namespace Stride.Physics
{
    public class DebugPrimitive : IDebugPrimitive, IEnumerable<MeshDraw>
    {
        public readonly List<MeshDraw> MeshDraws = new List<MeshDraw>();

        public void Add(MeshDraw meshDraw)
        {
            MeshDraws.Add(meshDraw);
        }

        public IEnumerable<MeshDraw> GetMeshDraws()
        {
            return MeshDraws;
        }

        public IEnumerator<MeshDraw> GetEnumerator()
        {
            return MeshDraws.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
