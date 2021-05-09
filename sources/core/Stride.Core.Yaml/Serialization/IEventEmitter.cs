// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Interface used to write YAML events.
    /// </summary>
    public interface IEventEmitter
    {
        void StreamStart();

        void DocumentStart();

        void Emit(AliasEventInfo eventInfo);

        void Emit(ScalarEventInfo eventInfo);

        void Emit(MappingStartEventInfo eventInfo);

        void Emit(MappingEndEventInfo eventInfo);

        void Emit(SequenceStartEventInfo eventInfo);

        void Emit(SequenceEndEventInfo eventInfo);

        void Emit(ParsingEvent parsingEvent);

        void DocumentEnd();

        void StreamEnd();
    }
}