// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.Yaml.Events
{
    /// <summary>
    /// Defines the event types. This allows for simpler type comparisons.
    /// </summary>
    internal enum EventType
    {
        /// <summary>
        /// An empty event.
        /// </summary>
        YAML_NO_EVENT,

        /// <summary>
        /// A STREAM-START event.
        /// </summary>
        YAML_STREAM_START_EVENT,

        /// <summary>
        /// A STREAM-END event.
        /// </summary>
        YAML_STREAM_END_EVENT,

        /// <summary>
        /// A DOCUMENT-START event.
        /// </summary>
        YAML_DOCUMENT_START_EVENT,

        /// <summary>
        /// A DOCUMENT-END event.
        /// </summary>
        YAML_DOCUMENT_END_EVENT,

        /// <summary>
        /// An ALIAS event.
        /// </summary>
        YAML_ALIAS_EVENT,

        /// <summary>
        /// A SCALAR event.
        /// </summary>
        YAML_SCALAR_EVENT,

        /// <summary>
        /// A SEQUENCE-START event.
        /// </summary>
        YAML_SEQUENCE_START_EVENT,

        /// <summary>
        /// A SEQUENCE-END event.
        /// </summary>
        YAML_SEQUENCE_END_EVENT,

        /// <summary>
        /// A MAPPING-START event.
        /// </summary>
        YAML_MAPPING_START_EVENT,

        /// <summary>
        /// A MAPPING-END event.
        /// </summary>
        YAML_MAPPING_END_EVENT,
    }
}