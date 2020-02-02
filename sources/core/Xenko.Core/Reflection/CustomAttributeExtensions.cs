// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xenko.Core.Annotations;

namespace Xenko.Core.Reflection
{
    public static class CustomAttributeExtensions
    {
        public static T GetCustomAttributeEx<T>([NotNull] this Assembly assembly) where T : Attribute
        {
            return (T)GetCustomAttributeEx(assembly, typeof(T));
        }

        public static Attribute GetCustomAttributeEx([NotNull] this Assembly assembly, [NotNull] Type attributeType)
        {
            return assembly.GetCustomAttribute(attributeType);
        }

        public static IEnumerable<Attribute> GetCustomAttributesEx([NotNull] this Assembly assembly, [NotNull] Type attributeType)
        {
            return assembly.GetCustomAttributes(attributeType);
        }

        [NotNull]
        public static IEnumerable<T> GetCustomAttributesEx<T>([NotNull] this Assembly assembly) where T : Attribute
        {
            return GetCustomAttributesEx(assembly, typeof(T)).Cast<T>();
        }
    }
}
