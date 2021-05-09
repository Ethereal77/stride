// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization
{
    public abstract class EventInfo
    {
        public object SourceValue { get; private set; }
        public Type SourceType { get; private set; }

        protected EventInfo(object sourceValue, Type sourceType)
        {
            SourceValue = sourceValue;
            SourceType = sourceType;
        }
    }

    public class AliasEventInfo : EventInfo
    {
        public AliasEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }

        public string Alias { get; set; }
    }

    public class ObjectEventInfo : EventInfo
    {
        protected ObjectEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }

        public string Anchor { get; set; }
        public string Tag { get; set; }
    }

    public sealed class ScalarEventInfo : ObjectEventInfo
    {
        public ScalarEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }

        public string RenderedValue { get; set; }
        public ScalarStyle Style { get; set; }
        public bool IsPlainImplicit { get; set; }
        public bool IsQuotedImplicit { get; set; }
    }

    public sealed class MappingStartEventInfo : ObjectEventInfo
    {
        public MappingStartEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }

        public bool IsImplicit { get; set; }
        public DataStyle Style { get; set; }
    }

    public sealed class MappingEndEventInfo : EventInfo
    {
        public MappingEndEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }
    }

    public sealed class SequenceStartEventInfo : ObjectEventInfo
    {
        public SequenceStartEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }

        public bool IsImplicit { get; set; }
        public DataStyle Style { get; set; }
    }

    public sealed class SequenceEndEventInfo : EventInfo
    {
        public SequenceEndEventInfo(object sourceValue, Type sourceType)
            : base(sourceValue, sourceType)
        {
        }
    }
}