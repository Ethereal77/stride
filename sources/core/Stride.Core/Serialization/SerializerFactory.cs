// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Storage;

namespace Stride.Core.Serialization
{
    /// <summary>
    /// Used as a fallback when <see cref="SerializerSelector.GetSerializer"/> didn't find anything.
    /// </summary>
    public abstract class SerializerFactory
    {
        public abstract DataSerializer GetSerializer(SerializerSelector selector, ref ObjectId typeId);
        public abstract DataSerializer GetSerializer(SerializerSelector selector, Type type);
    }
}
