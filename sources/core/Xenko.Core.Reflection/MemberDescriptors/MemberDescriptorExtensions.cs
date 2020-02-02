// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Reflection;

namespace Xenko.Core.Reflection
{
    /// <summary>
    /// Extension methods for <see cref="IMemberDescriptor"/>
    /// </summary>
    public static class MemberDescriptorExtensions
    {
        public static int CompareMetadataTokenWith(this MemberInfo leftMember, MemberInfo rightMember)
        {
            if (leftMember == null)
                return -1;
            if (rightMember == null)
                return 1;

            // If declared in same type, order by metadata token
            if (leftMember.DeclaringType == rightMember.DeclaringType)
                return leftMember.MetadataToken.CompareTo(rightMember.MetadataToken);

            // Otherwise, put base class first
            return (leftMember.DeclaringType.IsSubclassOf(rightMember.DeclaringType)) ? 1 : -1;
        }
    }
}
