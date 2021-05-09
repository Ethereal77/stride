// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   Generates a serializer type for a given <c>enum</c> type.
    /// </summary>
    public class CecilEnumSerializerFactory : ICecilSerializerFactory
    {
        private readonly TypeReference genericEnumSerializerType;

        public CecilEnumSerializerFactory(TypeReference genericEnumSerializerType)
        {
            this.genericEnumSerializerType = genericEnumSerializerType;
        }

        public TypeReference GetSerializer(TypeReference objectType)
        {
            var resolvedObjectType = objectType.Resolve();
            if (resolvedObjectType != null && resolvedObjectType.IsEnum)
            {
                return genericEnumSerializerType.MakeGenericType(objectType);
            }

            return null;
        }
    }
}
