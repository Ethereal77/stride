// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core.Serialization;
using Stride.Core.Storage;
using Stride.Rendering;

namespace Stride.Shaders
{
    class ParameterKeyHashSerializer : DataSerializer<ParameterKey>
    {
        public unsafe override void Serialize(ref ParameterKey obj, ArchiveMode mode, SerializationStream stream)
        {
            if (mode != ArchiveMode.Serialize)
                throw new InvalidOperationException();

            // Just use parameter key hash code
            // Hopefully there won't be any clash...
            fixed (ulong* objId = &obj.HashCode)
            {
                stream.Serialize(ref *objId);
            }
        }
    }
}
