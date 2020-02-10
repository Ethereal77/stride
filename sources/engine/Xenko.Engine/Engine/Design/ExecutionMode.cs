// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Engine.Design
{
    /// <summary>
    /// Describes the different execution mode of the engine.
    /// </summary>
    [Flags]
    public enum ExecutionMode
    {
        None = 0,
        Runtime = 1,
        Editor = 2,
        Thumbnail = 4,
        Preview = 8,
        EffectCompile = 16,
        All = Runtime | Editor | Thumbnail | Preview | EffectCompile,
    }
}
