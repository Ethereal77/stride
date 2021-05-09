// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Serialization
{
    /// <summary>
    /// A null serializer that can be used to add dummy serialization attributes.
    /// </summary>
    public class NullSerializer<T> : DataSerializer<T>
    {
        public override void Serialize(ref T obj, ArchiveMode mode, SerializationStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
