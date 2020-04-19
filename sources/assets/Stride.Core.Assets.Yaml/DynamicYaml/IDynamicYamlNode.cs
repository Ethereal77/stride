// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Yaml.Serialization;

namespace Xenko.Core.Yaml
{
    public interface IDynamicYamlNode
    {
        YamlNode Node { get; }
    }
}
