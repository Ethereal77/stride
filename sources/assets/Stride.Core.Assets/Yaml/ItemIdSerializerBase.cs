// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Reflection;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
{
    /// <summary>
    /// A base class to serialize <see cref="ItemId"/>.
    /// </summary>
    public abstract class ItemIdSerializerBase : AssetScalarSerializerBase
    {
        /// <summary>
        /// A key used in properties of serialization contexts to notify whether an override flag should be appened when serializing the related <see cref="ItemId"/>.
        /// </summary>
        public static PropertyKey<string> OverrideInfoKey = new PropertyKey<string>("OverrideInfo", typeof(ItemIdSerializer));

        /// <inheritdoc/>
        public override string ConvertTo(ref ObjectContext objectContext)
        {
            var result = ((ItemId)objectContext.Instance).ToString();
            string overrideInfo;
            if (objectContext.SerializerContext.Properties.TryGetValue(OverrideInfoKey, out overrideInfo))
            {
                result += overrideInfo;
                objectContext.SerializerContext.Properties.Remove(OverrideInfoKey);
            }
            return result;
        }

    }
}
