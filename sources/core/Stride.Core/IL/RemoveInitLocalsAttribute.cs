// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.IL
{
    /// <summary>
    /// Using this optimization attribute will prevent local variables in this method to be zero-ed in the prologue (if the runtime supports it).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RemoveInitLocalsAttribute : Attribute
    {
    }
}
