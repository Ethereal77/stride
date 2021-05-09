// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Updater
{
    /// <summary>
    /// Defines this member should be supported by <see cref="UpdateEngine"/>
    /// even if <see cref="Stride.Core.DataMemberIgnoreAttribute"/> is applied on it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DataMemberUpdatableAttribute : Attribute
    {
    }
}
