// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   Provides a method to enumerates required subtypes a given serializer will use internally.
    ///   This is useful for generation of serialization assemblies, when AOT is performed (all generic serializers must be available).
    /// </summary>
    public interface ICecilSerializerDependency
    {
        /// <summary>
        ///   Enumerates the types this serializer requires.
        /// </summary>
        /// <param name="serializerType">Type of the serializer.</param>
        /// <returns>Types this serializer requires.</returns>
        IEnumerable<TypeReference> EnumerateSubTypesFromSerializer(TypeReference serializerType);
    }
}
