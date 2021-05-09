// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization.Serializers
{
    /// <summary>
    /// This serializer is responsible to route to a specific serializer.
    /// </summary>
    public class RoutingSerializer : ChainedSerializer, IYamlSerializable
    {
        private readonly ISerializerFactorySelector serializerSelector;

        public RoutingSerializer(ISerializerFactorySelector serializerSelector)
        {
            if (serializerSelector == null) throw new ArgumentNullException(nameof(serializerSelector));
            this.serializerSelector = serializerSelector;
        }

        public sealed override object ReadYaml(ref ObjectContext objectContext)
        {
            // If value is not null, use its TypeDescriptor otherwise use expected type descriptor
            var instance = objectContext.Instance;
            var typeDescriptorOfValue = instance != null ? objectContext.SerializerContext.FindTypeDescriptor(instance.GetType()) : objectContext.Descriptor;

            var serializer = serializerSelector.GetSerializer(objectContext.SerializerContext, typeDescriptorOfValue);
            return serializer.ReadYaml(ref objectContext);
        }

        public sealed override void WriteYaml(ref ObjectContext objectContext)
        {
            var serializer = serializerSelector.GetSerializer(objectContext.SerializerContext, objectContext.Descriptor);
            serializer.WriteYaml(ref objectContext);
        }
    }
}
