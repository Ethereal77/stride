// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Annotations;
using Stride.Core.Storage;
using Stride.Core.Yaml.Events;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// A Yaml serializer for <see cref="ObjectId"/>
    /// </summary>
    [YamlSerializerFactory(YamlSerializerFactoryAttribute.Default)]
    internal class ObjectIdSerializer : AssetScalarSerializerBase
    {
        public override bool CanVisit(Type type)
        {
            return type == typeof(ObjectId);
        }

        [NotNull]
        public override object ConvertFrom(ref ObjectContext context, [NotNull] Scalar fromScalar)
        {
            ObjectId objectId;
            ObjectId.TryParse(fromScalar.Value, out objectId);
            return objectId;
        }

        [NotNull]
        public override string ConvertTo(ref ObjectContext objectContext)
        {
            return ((ObjectId)objectContext.Instance).ToString();
        }
    }
}
