// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;

namespace Xenko.Physics
{
    [Flags]
    public enum PhysicsEngineFlags
    {
        None = 0x0,

        CollisionsOnly = 0x1,

        SoftBodySupport = 0x2,

        MultiThreaded = 0x4,

        UseHardwareWhenPossible = 0x8,

        // Typo before 3.1 (https://github.com/xenko3d/xenko/issues/152)
        [DataAlias("ContinuosCollisionDetection")]
        ContinuousCollisionDetection = 0x10,
    }
}
