// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// A naming convention where all members are outputed as-is.
    /// </summary>
    public class DefaultNamingConvention : IMemberNamingConvention
    {
        public StringComparer Comparer { get { return StringComparer.Ordinal; } }

        public string Convert(string name)
        {
            return name;
        }
    }
}