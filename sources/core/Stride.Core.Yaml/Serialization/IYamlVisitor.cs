// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Defines the method needed to be able to visit Yaml elements.
    /// </summary>
    public interface IYamlVisitor
    {
        /// <summary>
        /// Visits a <see cref="YamlStream"/>.
        /// </summary>
        /// <param name="stream">
        /// The <see cref="YamlStream"/> that is being visited.
        /// </param>
        void Visit(YamlStream stream);

        /// <summary>
        /// Visits a <see cref="YamlDocument"/>.
        /// </summary>
        /// <param name="document">
        /// The <see cref="YamlDocument"/> that is being visited.
        /// </param>
        void Visit(YamlDocument document);

        /// <summary>
        /// Visits a <see cref="YamlScalarNode"/>.
        /// </summary>
        /// <param name="scalar">
        /// The <see cref="YamlScalarNode"/> that is being visited.
        /// </param>
        void Visit(YamlScalarNode scalar);

        /// <summary>
        /// Visits a <see cref="YamlSequenceNode"/>.
        /// </summary>
        /// <param name="sequence">
        /// The <see cref="YamlSequenceNode"/> that is being visited.
        /// </param>
        void Visit(YamlSequenceNode sequence);

        /// <summary>
        /// Visits a <see cref="YamlMappingNode"/>.
        /// </summary>
        /// <param name="mapping">
        /// The <see cref="YamlMappingNode"/> that is being visited.
        /// </param>
        void Visit(YamlMappingNode mapping);
    }
}