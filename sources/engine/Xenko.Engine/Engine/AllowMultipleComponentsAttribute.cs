// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Engine
{
    /// <summary>
    /// Allows a component of the same type to be added multiple time to the same entity (default is <c>false</c>)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowMultipleComponentsAttribute : EntityComponentAttributeBase
    {
    }
}
