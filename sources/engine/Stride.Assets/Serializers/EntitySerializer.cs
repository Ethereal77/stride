// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Serializers;
using Stride.Core.Reflection;
using Stride.Core.Yaml.Serialization;
using Stride.Core.Yaml.Serialization.Serializers;
using Stride.Engine;

namespace Stride.Assets.Serializers
{
    /// <summary>
    /// A serializer for the <see cref="Entity"/> type.
    /// </summary>
    [YamlSerializerFactory(YamlAssetProfile.Name)]
    public class EntitySerializer : ObjectSerializer
    {
        /// <inheritdoc/>
        public override IYamlSerializable TryCreate(SerializerContext context, ITypeDescriptor typeDescriptor)
        {
            return typeDescriptor.Type == typeof(Entity) ? this : null;
        }

        /// <inheritdoc/>
        protected override void CreateOrTransformObject(ref ObjectContext objectContext)
        {
            base.CreateOrTransformObject(ref objectContext);

            // When deserializing, we don't keep the TransformComponent created when the Entity is created
            if (!objectContext.SerializerContext.IsSerializing)
            {
                var entity = (Entity)objectContext.Instance;
                entity.Components.Clear();
            }
        }
    }
}
