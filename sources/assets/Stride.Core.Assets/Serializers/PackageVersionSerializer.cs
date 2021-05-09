// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Yaml;
using Stride.Core.Yaml.Events;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Assets.Serializers
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
