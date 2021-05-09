// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   Generates a serializer type for a given array type.
    /// </summary>
    public class CecilArraySerializerFactory : ICecilSerializerFactory
    {
        private readonly TypeReference genericArraySerializerType;

        public CecilArraySerializerFactory(TypeReference genericArraySerializerType)
        {
            this.genericArraySerializerType = genericArraySerializerType;
        }

        public TypeReference GetSerializer(TypeReference objectType)
        {
            if (objectType.IsArray)
            {
                return genericArraySerializerType.MakeGenericType(((ArrayType)objectType).ElementType);
            }

            return null;
        }
    }
}
