// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;

using Xenko.Core.Yaml.Serialization;

using Xunit;

namespace Xenko.Core.Yaml.Tests.Serialization
{
    public class ExceptionWithNestedSerialization
    {
        [Fact]
        public void NestedDocumentShouldDeserializeProperly()
        {
            var serializer = new Serializer(new SerializerSettings() {EmitDefaultValues = true});

            // serialize AMessage
            var tw = new StringWriter();
            serializer.Serialize(tw, new AMessage {Payload = new PayloadA {X = 5, Y = 6}});
            Dump.WriteLine(tw);

            // stick serialized AMessage in envelope and serialize it
            var e = new Env {Type = "some-type", Payload = tw.ToString()};

            tw = new StringWriter();
            serializer.Serialize(tw, e);
            Dump.WriteLine(tw);

            Dump.WriteLine("${0}$", e.Payload);

            var settings = new SerializerSettings();
            settings.RegisterAssembly(typeof(Env).Assembly);
            var deserializer = new Serializer(settings);
            // deserialize envelope
            var e2 = deserializer.Deserialize<Env>(new StringReader(tw.ToString()));

            Dump.WriteLine("${0}$", e2.Payload);

            // deserialize payload - fails if EmitDefaults is set
            var message = deserializer.Deserialize<AMessage>(e2.Payload);
            Assert.NotNull(message.Payload);
            Assert.Equal(5, message.Payload.X);
            Assert.Equal(6, message.Payload.Y);
        }

        public class Env
        {
            public string Type { get; set; }
            public string Payload { get; set; }
        }

        public class Message<TPayload>
        {
            public int id { get; set; }
            public TPayload Payload { get; set; }
        }

        public class PayloadA
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class AMessage : Message<PayloadA>
        {
        }
    }
}
