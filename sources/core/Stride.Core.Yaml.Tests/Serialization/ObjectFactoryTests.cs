// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System.IO;

using Stride.Core.Yaml.Serialization;

using Xunit;

namespace Stride.Core.Yaml.Tests.Serialization
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
