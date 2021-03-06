// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;

namespace Stride.Core.Serialization
{
    public class SerializerContext
    {
#pragma warning disable SA1401 // Fields must be private
        public PropertyContainer Tags;
#pragma warning restore SA1401 // Fields must be private

        public SerializerContext()
        {
            SerializerSelector = SerializerSelector.Default;
            Tags = new PropertyContainer(this);
        }

        /// <summary>
        /// Gets or sets the serializer.
        /// </summary>
        /// <value>
        /// The serializer.
        /// </value>
        public SerializerSelector SerializerSelector { get; set; }

        public T Get<T>([NotNull] PropertyKey<T> key)
        {
            return Tags.Get(key);
        }

        public void Set<T>([NotNull] PropertyKey<T> key, T value)
        {
            Tags.SetObject(key, value);
        }
    }
}
