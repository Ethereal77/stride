// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Diagnostics;

namespace Xenko.Physics
{
    public static class PhysicsProfilingKeys
    {
        public static ProfilingKey SimulationProfilingKey = new ProfilingKey("Physics Simulation");

        public static ProfilingKey ContactsProfilingKey = new ProfilingKey("Physics Contacts");

        public static ProfilingKey CharactersProfilingKey = new ProfilingKey(SimulationProfilingKey, "Physics Characters");
    }
}
