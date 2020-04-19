// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2016 JetBrains http://www.jetbrains.com
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Annotations
{
    /// <summary>
    /// Can be appplied to symbols of types derived from <see cref="System.Collections.Generic.IEnumerable{T}"/>
    /// as well as to symbols of <see cref="System.Threading.Tasks.Task"/> and <see cref="Lazy{T}"/> classes
    /// to indicate that the value of a collection item, of the Task.Result property or of the Lazy.Value property
    /// can be <c>null</c>.
    /// </summary>
    [AttributeUsage(
         AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
         AttributeTargets.Delegate | AttributeTargets.Field)]
    public sealed class ItemCanBeNullAttribute : Attribute { }
}
