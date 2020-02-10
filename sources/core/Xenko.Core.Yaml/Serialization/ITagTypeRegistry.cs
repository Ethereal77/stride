// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Reflection;

using Xenko.Core.Reflection;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// Prodives tag discovery from a type and type discovery from a tag.
    /// </summary>
    public interface ITagTypeRegistry : ITagTypeResolver
    {
        /// <summary>
        /// Registers an assembly when trying to resolve types. All types
        /// having <see cref="DataMemberAttribute" /> will be registered
        /// automatically.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attributeRegistry">The attribute registry to use when quering for <see cref="DataMemberAttribute"/>.</param>
        void RegisterAssembly(Assembly assembly, IAttributeRegistry attributeRegistry);

        /// <summary>
        /// Register a mapping between a tag and a type.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="type">The type.</param>
        /// <param name="alias">if set to <c>true</c> the specified tag is an alias to an existing type that has already a tag associated with it (remap).</param>
        void RegisterTagMapping(string tag, Type type, bool alias);
    }
}
