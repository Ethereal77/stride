// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Xenko.Core;
using Xenko.Core.Yaml;
using Xenko.Core.Yaml.Events;
using Xenko.Core.Yaml.Serialization;

namespace Xenko.Core.Assets.Serializers
{
    /// <summary>
    /// A Yaml serializer for <see cref="PackageVersion"/>
    /// </summary>
    [YamlSerializerFactory(YamlAssetProfile.Name)]
    internal class PackageVersionSerializer : AssetScalarSerializerBase
    {
        public override bool CanVisit(Type type)
        {
            return typeof(PackageVersion).IsAssignableFrom(type);
        }

        public override object ConvertFrom(ref ObjectContext context, Scalar fromScalar)
        {
            PackageVersion packageVersion;
            if (!PackageVersion.TryParse(fromScalar.Value, out packageVersion))
            {
                throw new YamlException(fromScalar.Start, fromScalar.End, "Invalid version format. Unable to decode [{0}]".ToFormat(fromScalar.Value));
            }
            return packageVersion;
        }

        public override string ConvertTo(ref ObjectContext objectContext)
        {
            return objectContext.Instance.ToString();
        }
    }
}
