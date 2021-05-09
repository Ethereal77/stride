// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization
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