// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Assets.Scripts
{
    /// <summary>
    /// <see cref="Block.Slots"/> need to be recomputed if a member with this attribute is changed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RegenerateSlotsAttribute : Attribute
    {
        
    }
}
