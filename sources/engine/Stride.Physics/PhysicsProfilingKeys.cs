// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Diagnostics;

namespace Stride.Physics
{
    public static class PhysicsProfilingKeys
    {
        public static ProfilingKey SimulationProfilingKey = new ProfilingKey("Physics Simulation");

        public static ProfilingKey ContactsProfilingKey = new ProfilingKey("Physics Contacts");

        public static ProfilingKey CharactersProfilingKey = new ProfilingKey(SimulationProfilingKey, "Physics Characters");
    }
}
