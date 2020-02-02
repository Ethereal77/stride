// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2013, Milosz Krajewski
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.LZ4
{
    internal interface ILZ4Service
    {
        string CodecName { get; }
        int Encode(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength);
        int EncodeHC(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength);
        int Decode(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength, bool knownOutputLength);
    }
}
