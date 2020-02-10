// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

using Xenko.Core.Annotations;

namespace Xenko.Core.Serialization
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
