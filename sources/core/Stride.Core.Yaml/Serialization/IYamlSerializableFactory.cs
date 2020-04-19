// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Reflection;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// A factory of <see cref="IYamlSerializable"/>
    /// </summary>
    [AssemblyScan]
    public interface IYamlSerializableFactory
    {
        /// <summary>
        /// Try to create a <see cref="IYamlSerializable"/> or return null if not supported for a particular .NET typeDescriptor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="typeDescriptor">The typeDescriptor.</param>
        /// <returns>If supported, return an instance of <see cref="IYamlSerializable"/> else return <c>null</c>.</returns>
        IYamlSerializable TryCreate(SerializerContext context, ITypeDescriptor typeDescriptor);
    }
}
