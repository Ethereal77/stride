// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Allows an object to customize how it is serialized and deserialized.
    /// </summary>
    public interface IYamlSerializable
    {
        /// <summary>
        /// Reads this object's state from a YAML parser.
        /// </summary>
        /// <param name="objectContext"></param>
        /// <returns>A instance of the object deserialized from Yaml.</returns>
        object ReadYaml(ref ObjectContext objectContext);

        /// <summary>
        /// Writes the specified object context to a YAML emitter.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        void WriteYaml(ref ObjectContext objectContext);
    }
}