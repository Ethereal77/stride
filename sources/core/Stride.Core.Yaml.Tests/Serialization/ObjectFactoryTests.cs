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
    public class ObjectFactoryTests
    {
        public class FooBase
        {
        }

        public class FooDerived : FooBase
        {
        }

        [Fact]
        public void NotSpecifyingObjectFactoryUsesDefault()
        {
            var settings = new SerializerSettings();
            settings.RegisterTagMapping("!foo", typeof(FooBase));
            var serializer = new Serializer(settings);
            var result = serializer.Deserialize(new StringReader("!foo {}"));

            Assert.True(result is FooBase);
        }

        [Fact]
        public void ObjectFactoryIsInvoked()
        {
            var settings = new SerializerSettings()
            {
                ObjectFactory = new LambdaObjectFactory(t => new FooDerived(), new DefaultObjectFactory())
            };
            settings.RegisterTagMapping("!foo", typeof(FooBase));

            var serializer = new Serializer(settings);

            var result = serializer.Deserialize(new StringReader("!foo {}"));

            Assert.True(result is FooDerived);
        }
    }
}
