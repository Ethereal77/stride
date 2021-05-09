// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.IO;

namespace Stride.Core.Storage
{
    public class DigestStream : OdbStreamWriter
    {
        private ObjectIdBuilder builder = new ObjectIdBuilder();

        public override ObjectId CurrentHash
        {
            get
            {
                return builder.ComputeHash();
            }
        }

        public DigestStream(Stream stream) : base(stream, null)
        {
        }

        internal DigestStream(Stream stream, string temporaryName) : base(stream, temporaryName)
        {
        }

        public void Reset()
        {
            Position = 0;
            builder.Reset();
        }

        public override void WriteByte(byte value)
        {
            builder.WriteByte(value);
            stream.WriteByte(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            builder.Write(buffer, offset, count);
            stream.Write(buffer, offset, count);
        }
    }
}
