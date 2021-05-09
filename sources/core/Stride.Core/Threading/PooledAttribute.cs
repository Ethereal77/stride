// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Threading
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
