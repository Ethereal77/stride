// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.IL
{
    /// <summary>
    /// Using this optimization attribute will prevent local variables in this method to be zero-ed in the prologue (if the runtime supports it).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RemoveInitLocalsAttribute : Attribute
    {
    }
}
