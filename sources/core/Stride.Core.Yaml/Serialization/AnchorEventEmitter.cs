// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization
{
    internal class AnchorEventEmitter : ChainedEventEmitter
    {
        private readonly List<object> events = new List<object>();
        private readonly HashSet<string> alias = new HashSet<string>();

        public AnchorEventEmitter(IEventEmitter nextEmitter) : base(nextEmitter)
        {
        }

        public override void Emit(AliasEventInfo eventInfo)
        {
            alias.Add(eventInfo.Alias);
            events.Add(eventInfo);
        }

        public override void Emit(ScalarEventInfo eventInfo)
        {
            events.Add(eventInfo);
        }

        public override void Emit(MappingStartEventInfo eventInfo)
        {
            events.Add(eventInfo);
        }

        public override void Emit(MappingEndEventInfo eventInfo)
        {
            events.Add(eventInfo);
        }

        public override void Emit(SequenceStartEventInfo eventInfo)
        {
            events.Add(eventInfo);
        }

        public override void Emit(SequenceEndEventInfo eventInfo)
        {
            events.Add(eventInfo);
        }

        public override void Emit(ParsingEvent parsingEvent)
        {
            events.Add(parsingEvent);
        }

        public override void DocumentEnd()
        {
            // remove all unused anchor
            foreach (var objectEventInfo in events.OfType<ObjectEventInfo>())
            {
                if (objectEventInfo.Anchor != null && !alias.Contains(objectEventInfo.Anchor))
                {
                    objectEventInfo.Anchor = null;
                }
            }

            // Flush all events to emitter.
            foreach (var evt in events)
            {
                if (evt is AliasEventInfo)
                {
                    nextEmitter.Emit((AliasEventInfo) evt);
                }
                else if (evt is ScalarEventInfo)
                {
                    nextEmitter.Emit((ScalarEventInfo) evt);
                }
                else if (evt is MappingStartEventInfo)
                {
                    nextEmitter.Emit((MappingStartEventInfo) evt);
                }
                else if (evt is MappingEndEventInfo)
                {
                    nextEmitter.Emit((MappingEndEventInfo) evt);
                }
                else if (evt is SequenceStartEventInfo)
                {
                    nextEmitter.Emit((SequenceStartEventInfo) evt);
                }
                else if (evt is SequenceEndEventInfo)
                {
                    nextEmitter.Emit((SequenceEndEventInfo) evt);
                }
                else if (evt is ParsingEvent)
                {
                    nextEmitter.Emit((ParsingEvent) evt);
                }
            }

            nextEmitter.DocumentEnd();
        }
    }
}