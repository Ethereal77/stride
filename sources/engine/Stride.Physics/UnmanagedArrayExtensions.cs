// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Physics
{
    public static class UnmanagedArrayExtensions
    {
        /// <summary>
        ///   Fill the array with specific value.
        /// </summary>
        /// <typeparam name="T">The type param of UnmanagedArray.</typeparam>
        /// <param name="unmanagedArray">The destination to fill.</param>
        /// <param name="value">The value used to fill.</param>
        /// <param name="index">The start index of the destination to fill.</param>
        /// <param name="fillLength">The filling length.</param>
        public static void Fill<T>(this UnmanagedArray<T> unmanagedArray, T value, int index, int fillLength) where T : struct
        {
            if (unmanagedArray is null)
                throw new ArgumentNullException(nameof(unmanagedArray));

            var length = unmanagedArray.Length;
            var endIndex = index + fillLength;

            if (length <= index)
                throw new IndexOutOfRangeException(nameof(index));
            if (length < endIndex)
                throw new ArgumentException($"{ nameof(unmanagedArray) }.{ nameof(unmanagedArray.Length) } is not enough to fill.");

            for (int i = index; i < endIndex; ++i)
            {
                unmanagedArray[i] = value;
            }
        }
    }
}
