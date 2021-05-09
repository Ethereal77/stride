// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Core.Yaml.Events;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// A Yaml serializer for <see cref="Guid"/>
    /// </summary>
    [YamlSerializerFactory(YamlSerializerFactoryAttribute.Default)]
    internal class GuidSerializer : AssetScalarSerializerBase
    {
        static GuidSerializer()
        {
            TypeDescriptorFactory.Default.AttributeRegistry.Register(typeof(Guid), new DataContractAttribute("Guid"));
        }

        public override bool CanVisit(Type type)
        {
            return type == typeof(Guid);
        }

        [NotNull]
        public override object ConvertFrom(ref ObjectContext context, [NotNull] Scalar fromScalar)
        {
            Guid guid;
            Guid.TryParse(fromScalar.Value, out guid);
            return guid;
        }

        [NotNull]
        public override string ConvertTo(ref ObjectContext objectContext)
        {
            return ((Guid)objectContext.Instance).ToString();
        }
    }
}
