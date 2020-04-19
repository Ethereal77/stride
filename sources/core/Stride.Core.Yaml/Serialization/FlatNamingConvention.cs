// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Text.RegularExpressions;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// A naming convention where all members are transformed from`CamelCase` to `camel_case`.
    /// </summary>
    public class FlatNamingConvention : IMemberNamingConvention
    {
        // Code taken from dotliquid/RubyNamingConvention.cs
        private readonly Regex regex1 = new Regex(@"([A-Z]+)([A-Z][a-z])");
        private readonly Regex regex2 = new Regex(@"([a-z\d])([A-Z])");

        public StringComparer Comparer { get { return StringComparer.OrdinalIgnoreCase; } }

        public string Convert(string name)
        {
            return regex2.Replace(regex1.Replace(name, "$1_$2"), "$1_$2").ToLowerInvariant();
        }
    }
}