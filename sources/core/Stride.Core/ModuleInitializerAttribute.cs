// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core
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
