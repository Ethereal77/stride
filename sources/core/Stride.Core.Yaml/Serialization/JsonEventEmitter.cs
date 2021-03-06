// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization
{
    internal sealed class JsonEventEmitter : ChainedEventEmitter
    {
        public JsonEventEmitter(IEventEmitter nextEmitter)
            : base(nextEmitter)
        {
        }

        public override void Emit(AliasEventInfo eventInfo)
        {
            throw new NotSupportedException("Aliases are not supported in JSON");
        }

        public override void Emit(ScalarEventInfo eventInfo)
        {
            eventInfo.IsPlainImplicit = true;
            eventInfo.Style = ScalarStyle.Plain;

            var typeCode = eventInfo.SourceValue != null
                ? Type.GetTypeCode(eventInfo.SourceType)
                : TypeCode.Empty;

            switch (typeCode)
            {
                case TypeCode.String:
                case TypeCode.Char:
                    eventInfo.Style = ScalarStyle.DoubleQuoted;
                    break;
                case TypeCode.Empty:
                    eventInfo.RenderedValue = "null";
                    break;
            }

            base.Emit(eventInfo);
        }

        public override void Emit(MappingStartEventInfo eventInfo)
        {
            eventInfo.Style = DataStyle.Compact;

            base.Emit(eventInfo);
        }

        public override void Emit(SequenceStartEventInfo eventInfo)
        {
            eventInfo.Style = DataStyle.Compact;

            base.Emit(eventInfo);
        }
    }
}