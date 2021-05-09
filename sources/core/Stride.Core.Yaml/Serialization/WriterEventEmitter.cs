// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization
{
    internal sealed class WriterEventEmitter : IEventEmitter
    {
        private readonly IEmitter emitter;

        public WriterEventEmitter(IEmitter emitter)
        {
            this.emitter = emitter;
        }

        void IEventEmitter.StreamStart()
        {
            emitter.Emit(new StreamStart());
        }

        void IEventEmitter.DocumentStart()
        {
            emitter.Emit(new DocumentStart());
        }

        void IEventEmitter.Emit(AliasEventInfo eventInfo)
        {
            emitter.Emit(new AnchorAlias(eventInfo.Alias));
        }

        void IEventEmitter.Emit(ScalarEventInfo eventInfo)
        {
            emitter.Emit(new Scalar(eventInfo.Anchor, eventInfo.Tag, eventInfo.RenderedValue, eventInfo.Style, eventInfo.IsPlainImplicit, eventInfo.IsQuotedImplicit));
        }

        void IEventEmitter.Emit(MappingStartEventInfo eventInfo)
        {
            emitter.Emit(new MappingStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
        }

        void IEventEmitter.Emit(MappingEndEventInfo eventInfo)
        {
            emitter.Emit(new MappingEnd());
        }

        void IEventEmitter.Emit(SequenceStartEventInfo eventInfo)
        {
            emitter.Emit(new SequenceStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
        }

        void IEventEmitter.Emit(SequenceEndEventInfo eventInfo)
        {
            emitter.Emit(new SequenceEnd());
        }

        public void Emit(ParsingEvent parsingEvent)
        {
            emitter.Emit(parsingEvent);
        }

        void IEventEmitter.DocumentEnd()
        {
            emitter.Emit(new DocumentEnd(true));
        }

        void IEventEmitter.StreamEnd()
        {
            emitter.Emit(new StreamEnd());
        }
    }
}