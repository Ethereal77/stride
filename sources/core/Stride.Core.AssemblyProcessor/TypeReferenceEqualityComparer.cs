// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   <see cref="EqualityComparer{T}"/> for <see cref="TypeReference"/>, using <see cref="TypeReference.FullName"/> to compare.
    /// </summary>
    public class TypeReferenceEqualityComparer : EqualityComparer<TypeReference>
    {
        public new static readonly TypeReferenceEqualityComparer Default = new TypeReferenceEqualityComparer();

        public override bool Equals(TypeReference x, TypeReference y)
        {
            return x.FullName == y.FullName;
        }

        public override int GetHashCode(TypeReference obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
