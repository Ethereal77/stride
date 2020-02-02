// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ModuleInitializerAttribute : Attribute
    {
        public ModuleInitializerAttribute()
        {
        }

        public ModuleInitializerAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}
