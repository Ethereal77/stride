// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents a structure that defines a direct way of accesing a value parameter in an effect.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public struct ValueParameter<T> where T : struct
    {
        internal readonly int Offset;
        internal readonly int Count;

        internal ValueParameter(int offset, int count)
        {
            Offset = offset;
            Count = count;
        }
    }
}
