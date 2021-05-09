// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Particles.DebugDraw
{
    public enum DebugDrawShape
    {
        None        = 0,
        Sphere      = 1, // Spere centered at the origin O with a default radius of 1
        Cube        = 2, // Cube centered at the origin O with a side of 2, each corner at (-1,-1,-1) to (+1,+1,+1)
        Cone        = 3, // Cone, starting at the origin O, spreading out to (0, +1, 0) with a radius of 1
        Cylinder    = 4, // Cylinder, starting at the origin O, with a height of 1 and a radius of 1
        Torus       = 5, // Torus, centered around O, with a big radius of 1 and a small radius of 0.5
    }
}
