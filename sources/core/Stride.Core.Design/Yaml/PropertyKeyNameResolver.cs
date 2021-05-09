// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Text;

using Stride.Core.Annotations;
using Stride.Core.Yaml.Serialization;

namespace Stride.Core.Yaml
{
    public static class PropertyKeyNameResolver
    {
        [NotNull]
        public static string ComputePropertyKeyName([NotNull] ITagTypeResolver tagResolver, [NotNull] PropertyKey propertyKey)
        {
            var className = tagResolver.TagFromType(propertyKey.OwnerType);
            var sb = new StringBuilder(className.Length + 1 + propertyKey.Name.Length);

            sb.Append(className, 1, className.Length - 1); // Ignore initial '!'
            sb.Append('.');
            sb.Append(propertyKey.Name);
            return sb.ToString();
        }
    }
}
