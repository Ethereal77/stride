// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Comparer that is based on identity comparisons.
    /// </summary>
    public sealed class YamlNodeIdentityEqualityComparer : IEqualityComparer<YamlNode>
    {
        #region IEqualityComparer<YamlNode> Members

        /// <summary />
        public bool Equals(YamlNode x, YamlNode y)
        {
            return ReferenceEquals(x, y);
        }

        /// <summary />
        public int GetHashCode(YamlNode obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}