// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Core.Yaml.Serialization
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