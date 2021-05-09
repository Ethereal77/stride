// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Reflection;

using Stride.Core.Reflection;

namespace Stride.Core.Serialization.Serializers
{
    public class PropertyInfoSerializer : DataSerializer<PropertyInfo>
    {
        public override void Serialize(ref PropertyInfo propertyInfo, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                stream.Write(propertyInfo.DeclaringType.AssemblyQualifiedName);
                stream.Write(propertyInfo.Name);
            }
            else
            {
                var declaringTypeName = stream.ReadString();
                var propertyName = stream.ReadString();

                var ownerType = AssemblyRegistry.GetType(declaringTypeName);
                if (ownerType == null)
                    throw new InvalidOperationException("Could not find the appropriate type.");

                propertyInfo = ownerType.GetTypeInfo().GetDeclaredProperty(propertyName);
            }
        }
    }
}
