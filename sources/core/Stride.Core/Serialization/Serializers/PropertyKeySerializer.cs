// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Reflection;

using Xenko.Core.Reflection;

namespace Xenko.Core.Serialization.Serializers
{
    public class PropertyKeySerializer<T> : DataSerializer<T> where T : PropertyKey
    {
        public override void Serialize(ref T obj, ArchiveMode mode, SerializationStream stream)
        {
            if (mode == ArchiveMode.Serialize)
            {
                stream.Write(obj.Name);
                stream.Write(obj.OwnerType.AssemblyQualifiedName);
            }
            else
            {
                var parameterName = stream.ReadString();
                var ownerTypeName = stream.ReadString();
                var ownerType = AssemblyRegistry.GetType(ownerTypeName);

                obj = (T)ownerType.GetTypeInfo().GetDeclaredField(parameterName).GetValue(null);
            }
        }
    }
}
