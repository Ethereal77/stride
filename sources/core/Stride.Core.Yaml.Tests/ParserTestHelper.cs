// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Yaml.Events;
using Xenko.Core.Yaml.Tokens;

using AnchorAlias = Xenko.Core.Yaml.Events.AnchorAlias;
using DocumentEnd = Xenko.Core.Yaml.Events.DocumentEnd;
using DocumentStart = Xenko.Core.Yaml.Events.DocumentStart;
using Scalar = Xenko.Core.Yaml.Events.Scalar;
using StreamEnd = Xenko.Core.Yaml.Events.StreamEnd;
using StreamStart = Xenko.Core.Yaml.Events.StreamStart;

namespace Xenko.Core.Yaml.Tests
{
    public class ParserTestHelper : YamlTest
    {
        protected const bool Explicit = false;
        protected const bool Implicit = true;
        protected const string TagYaml = "tag:yaml.org,2002:";

        protected static readonly TagDirective[] DefaultTags = new[]
        {
            new TagDirective("!", "!"),
            new TagDirective("!!", TagYaml)
        };

        protected static StreamStart StreamStart { get { return new StreamStart(); } }

        protected static StreamEnd StreamEnd { get { return new StreamEnd(); } }

        protected DocumentStart DocumentStart(bool isImplicit)
        {
            return DocumentStart(isImplicit, null, DefaultTags);
        }

        protected DocumentStart DocumentStart(bool isImplicit, VersionDirective version, params TagDirective[] tags)
        {
            return new DocumentStart(version, new TagDirectiveCollection(tags), isImplicit);
        }

        protected VersionDirective Version(int major, int minor)
        {
            return new VersionDirective(new Version(major, minor));
        }

        protected TagDirective TagDirective(string handle, string prefix)
        {
            return new TagDirective(handle, prefix);
        }

        protected DocumentEnd DocumentEnd(bool isImplicit)
        {
            return new DocumentEnd(isImplicit);
        }

        protected Scalar PlainScalar(string text)
        {
            return new Scalar(null, null, text, ScalarStyle.Plain, true, false);
        }

        protected Scalar SingleQuotedScalar(string text)
        {
            return new Scalar(null, null, text, ScalarStyle.SingleQuoted, false, true);
        }

        protected Scalar DoubleQuotedScalar(string text)
        {
            return DoubleQuotedScalar(null, text);
        }

        protected Scalar ExplicitDoubleQuotedScalar(string tag, string text)
        {
            return DoubleQuotedScalar(tag, text, false);
        }

        protected Scalar DoubleQuotedScalar(string tag, string text, bool quotedImplicit = true)
        {
            return new Scalar(null, tag, text, ScalarStyle.DoubleQuoted, false, quotedImplicit);
        }

        protected Scalar LiteralScalar(string text)
        {
            return new Scalar(null, null, text, ScalarStyle.Literal, false, true);
        }

        protected Scalar FoldedScalar(string text)
        {
            return new Scalar(null, null, text, ScalarStyle.Folded, false, true);
        }

        protected SequenceStart BlockSequenceStart { get { return new SequenceStart(null, null, true, DataStyle.Normal); } }

        protected SequenceStart FlowSequenceStart { get { return new SequenceStart(null, null, true, DataStyle.Compact); } }

        protected SequenceStart AnchoredFlowSequenceStart(string anchor)
        {
            return new SequenceStart(anchor, null, true, DataStyle.Compact);
        }

        protected SequenceEnd SequenceEnd { get { return new SequenceEnd(); } }

        protected MappingStart BlockMappingStart { get { return new MappingStart(null, null, true, DataStyle.Normal); } }

        protected MappingStart TaggedBlockMappingStart(string tag)
        {
            return new MappingStart(null, tag, false, DataStyle.Normal);
        }

        protected MappingStart FlowMappingStart { get { return new MappingStart(null, null, true, DataStyle.Compact); } }

        protected MappingEnd MappingEnd { get { return new MappingEnd(); } }

        protected AnchorAlias AnchorAlias(string alias)
        {
            return new AnchorAlias(alias);
        }
    }
}