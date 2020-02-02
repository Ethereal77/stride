// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Yaml.Serialization
{
    /// <summary>
    /// An <see cref="IObjectFactory"/> that can be chained with another object factory;
    /// </summary>
    public class ChainedObjectFactory : IObjectFactory
    {
        private readonly IObjectFactory nextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainedObjectFactory"/> class.
        /// </summary>
        /// <param name="nextFactory">The next factory.</param>
        public ChainedObjectFactory(IObjectFactory nextFactory)
        {
            this.nextFactory = nextFactory;
        }

        public virtual object Create(Type type)
        {
            return nextFactory != null ? nextFactory.Create(type) : null;
        }
    }
}