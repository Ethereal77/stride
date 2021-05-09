// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2015 SharpYaml - Alexandre Mutel
// Copyright (c) 2008-2012 YamlDotNet - Antoine Aubry
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Yaml.Serialization
{
    /// <summary>
    /// Creates objects using a Func{Type,object}"/>.
    /// </summary>
    public sealed class LambdaObjectFactory : ChainedObjectFactory
    {
        private readonly Func<Type, object> factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaObjectFactory"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public LambdaObjectFactory(Func<Type, object> factory) : this(factory, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaObjectFactory" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="nextFactory">The next factory.</param>
        /// <exception cref="System.ArgumentNullException">factory</exception>
        public LambdaObjectFactory(Func<Type, object> factory, IObjectFactory nextFactory) : base(nextFactory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.factory = factory;
        }

        public override object Create(Type type)
        {
            return factory(type) ?? base.Create(type);
        }
    }
}