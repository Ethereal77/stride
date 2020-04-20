// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

using Stride.Core.Annotations;

namespace Stride.Core.Serialization
{
    public class HashSerializationWriter : BinarySerializationWriter
    {
        public HashSerializationWriter([NotNull] Stream outputStream) : base(outputStream)
        {
        }

        /// <inheritdoc/>
        public override unsafe void Serialize(ref string value)
        {
            fixed (char* bufferStart = value)
            {
                Serialize((IntPtr)bufferStart, sizeof(char) * value.Length);
            }
        }
    }
}
