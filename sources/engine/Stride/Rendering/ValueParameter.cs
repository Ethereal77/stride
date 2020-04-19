// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Rendering
{
    public struct ValueParameter<T> where T : struct
    {
        internal readonly int Offset;
        internal readonly int Count;

        internal ValueParameter(int offset, int count)
        {
            this.Offset = offset;
            this.Count = count;
        }
    }
}
