// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Yaml.Events;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Provided the base implementation for an IEventEmitter that is a
    /// decorator for another IEventEmitter.
    /// </summary>
    internal abstract class ChainedEventEmitter : IEventEmitter
    {
        protected readonly IEventEmitter nextEmitter;

        protected ChainedEventEmitter(IEventEmitter nextEmitter)
        {
            if (nextEmitter == null)
            {
                throw new ArgumentNullException("nextEmitter");
            }

            this.nextEmitter = nextEmitter;
        }

        public virtual void StreamStart()
        {
            nextEmitter.StreamStart();
        }

        public virtual void DocumentStart()
        {
            nextEmitter.DocumentStart();
        }

        public virtual void Emit(AliasEventInfo eventInfo)
        {
            nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(ScalarEventInfo eventInfo)
        {
            nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(MappingStartEventInfo eventInfo)
        {
            nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(MappingEndEventInfo eventInfo)
        {
            nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(SequenceStartEventInfo eventInfo)
        {
            nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(SequenceEndEventInfo eventInfo)
        {
            nextEmitter.Emit(eventInfo);
        }

        public virtual void Emit(ParsingEvent parsingEvent)
        {
            nextEmitter.Emit(parsingEvent);
        }

        public virtual void DocumentEnd()
        {
            nextEmitter.DocumentEnd();
        }

        public virtual void StreamEnd()
        {
            nextEmitter.StreamEnd();
        }
    }
}