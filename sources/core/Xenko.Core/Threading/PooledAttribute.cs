// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Threading
{
    /// <summary>
    /// Allows delegates passed as parameters to be allocated from a pool and recycled after the method call.
    /// To prevent recycling, use <see cref="PooledDelegateHelper.AddReference"/> and <see cref="PooledDelegateHelper.Release"/> to hoold onto references to the delegate.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PooledAttribute : Attribute
    {  
    }
}
