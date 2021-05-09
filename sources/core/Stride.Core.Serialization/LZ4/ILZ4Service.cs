// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2013, Milosz Krajewski
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.LZ4
{
    internal interface ILZ4Service
    {
        string CodecName { get; }
        int Encode(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength);
        int EncodeHC(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength);
        int Decode(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength, bool knownOutputLength);
    }
}
