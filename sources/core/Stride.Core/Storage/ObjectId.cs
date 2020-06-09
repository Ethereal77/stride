// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.InteropServices;

using Stride.Core.Annotations;

namespace Stride.Core.Storage
{
    /// <summary>
    ///   Represents a hash to uniquely identify a data object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
#if !STRIDE_ASSEMBLY_PROCESSOR
    [DataContract("ObjectId"),Serializable]
#endif
    public unsafe partial struct ObjectId : IEquatable<ObjectId>, IComparable<ObjectId>
    {
        // **************************************************************************************
        // NOTE: This file is shared with the AssemblyProcessor.
        //       If this file is modified, the AssemblyProcessor has to be recompiled separately.
        //       See build\Stride-AssemblyProcessor.sln
        // **************************************************************************************

        // MurmurHash3 hash size is 128 bits
        public const int HashSize = 16;
        public const int HashStringLength = HashSize * 2;
        private const int HashSizeInUInt = HashSize / sizeof(uint);
        private const string HexDigits = "0123456789abcdef";

        public static readonly ObjectId Empty = new ObjectId();

        private uint hash1, hash2, hash3, hash4;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ObjectId"/> struct.
        /// </summary>
        /// <param name="hash">The initial hash value for this <see cref="ObjectId"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is a <c>null</c> reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="hash"/>'s size doesn't match expected size (<see cref="HashSize"/>).</exception>
        public ObjectId([NotNull] byte[] hash)
        {
            if (hash is null)
                throw new ArgumentNullException(nameof(hash));
            if (hash.Length != HashSize)
                throw new ArgumentException($"The hash size doesn't match expected size of {HashSize}.");

            fixed (byte* hashSource = hash)
            {
                var hashSourceCurrent = (uint*) hashSource;
                hash1 = *hashSourceCurrent++;
                hash2 = *hashSourceCurrent++;
                hash3 = *hashSourceCurrent++;
                hash4 = *hashSourceCurrent;
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="ObjectId"/> struct.
        /// </summary>
        public ObjectId(uint hash1, uint hash2, uint hash3, uint hash4)
        {
            this.hash1 = hash1;
            this.hash2 = hash2;
            this.hash3 = hash3;
            this.hash4 = hash4;
        }

        public static unsafe explicit operator ObjectId(Guid guid)
        {
            return *(ObjectId*)&guid;
        }

        public static ObjectId Combine(ObjectId left, ObjectId right)
        {
            // NOTE: We don't carry (probably not worth the performance hit)
            return new ObjectId
            {
                hash1 = left.hash1 * 3 + right.hash1,
                hash2 = left.hash2 * 3 + right.hash2,
                hash3 = left.hash3 * 3 + right.hash3,
                hash4 = left.hash4 * 3 + right.hash4,
            };
        }

        public static void Combine(ref ObjectId left, ref ObjectId right, out ObjectId result)
        {
            // NOTE: We don't carry (probably not worth the performance hit)
            result = new ObjectId
            {
                hash1 = left.hash1 * 3 + right.hash1,
                hash2 = left.hash2 * 3 + right.hash2,
                hash3 = left.hash3 * 3 + right.hash3,
                hash4 = left.hash4 * 3 + right.hash4,
            };
        }

        /// <summary>
        ///   Performs an explicit conversion from <see cref="ObjectId"/> to <see cref="byte[]"/>.
        /// </summary>
        /// <param name="objectId">The <see cref="ObjectId"/> to convert.</param>
        /// <returns>The result of the conversion.</returns>
        [NotNull]
        public static explicit operator byte[](ObjectId objectId)
        {
            var result = new byte[HashSize];
            var hashSource = &objectId.hash1;
            fixed (byte* hashDest = result)
            {
                var hashSourceCurrent = (uint*)hashSource;
                var hashDestCurrent = (uint*)hashDest;
                for (var i = 0; i < HashSizeInUInt; ++i)
                    *hashDestCurrent++ = *hashSourceCurrent++;
            }
            return result;
        }

        /// <summary>
        ///   Implements the equality operator.
        /// </summary>
        /// <param name="left">The left-side <see cref="ObjectId"/> to compare.</param>
        /// <param name="right">The right-side <see cref="ObjectId"/> to compare.</param>
        /// <returns><c>true</c> if the two instances of <see cref="ObjectId"/> are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(ObjectId left, ObjectId right) => left.Equals(right);

        /// <summary>
        ///   Implements the inequality operator.
        /// </summary>
        /// <param name="left">The left-side <see cref="ObjectId"/> to compare.</param>
        /// <param name="right">The right-side <see cref="ObjectId"/> to compare.</param>
        /// <returns><c>true</c> if the two instances of <see cref="ObjectId"/> are different; <c>false</c> otherwise.</returns>
        public static bool operator !=(ObjectId left, ObjectId right) => !left.Equals(right);

        /// <summary>
        ///   Tries to parse an <see cref="ObjectId"/> from a <see cref="string"/>.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="result">The resulting parsed <see cref="ObjectId"/>.</param>
        /// <returns><c>true</c> if the parsing was successful; <c>false</c> otherwise.</returns>
        public static bool TryParse([NotNull] string input, out ObjectId result)
        {
            if (input.Length != HashStringLength)
            {
                result = Empty;
                return false;
            }

            var hash = new byte[HashSize];
            for (var i = 0; i < HashStringLength; i += 2)
            {
                var c1 = input[i];
                var c2 = input[i + 1];

                int digit1, digit2;
                if (((digit1 = HexDigits.IndexOf(c1)) == -1)
                    || ((digit2 = HexDigits.IndexOf(c2)) == -1))
                {
                    result = Empty;
                    return false;
                }

                hash[i >> 1] = (byte)((digit1 << 4) | digit2);
            }

            result = new ObjectId(hash);
            return true;
        }

        /// <inheritdoc/>
        public bool Equals(ObjectId other)
        {
            // Compare contents
            fixed (uint* xPtr = &hash1)
            {
                var x1 = xPtr;
                var y1 = &other.hash1;

                for (var i = 0; i < HashSizeInUInt; ++i)
                {
                    if (*x1++ != *y1++)
                        return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is ObjectId objectId)
                return Equals(objectId);
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            fixed (uint* objPtr = &hash1)
            {
                var obj1 = (int*) objPtr;
                return *obj1;
            }
        }

        /// <inheritdoc/>
        public int CompareTo(ObjectId other)
        {
            // Compare contents
            fixed (uint* xPtr = &hash1)
            {
                var x1 = xPtr;
                var y1 = &other.hash1;

                for (var i = 0; i < HashSizeInUInt; ++i)
                {
                    var compareResult = (*x1++).CompareTo(*y1++);
                    if (compareResult != 0)
                        return compareResult;
                }
            }

            return 0;
        }

        public override string ToString()
        {
            var c = new char[HashStringLength];

            fixed (uint* hashStart = &hash1)
            {
                var hashBytes = (byte*)hashStart;
                for (var i = 0; i < HashStringLength; ++i)
                {
                    var index0 = i >> 1;
                    var b = (byte)(hashBytes[index0] >> 4);
                    c[i++] = HexDigits[b];

                    b = (byte)(hashBytes[index0] & 0x0F);
                    c[i] = HexDigits[b];
                }
            }

            return new string(c);
        }

        /// <summary>
        ///   Gets a <see cref="Guid"/> representing this same object identifier.
        /// </summary>
        /// <returns>Guid.</returns>
        public Guid ToGuid()
        {
            fixed (void* hashStart = &hash1)
            {
                return *(Guid*) hashStart;
            }
        }

        /// <summary>
        ///   Returns a new instance of the <see cref="ObjectId"/> struct.
        /// </summary>
        /// <returns>New <see cref="ObjectId"/> with a random hash.</returns>
        public static ObjectId New() => FromBytes(Guid.NewGuid().ToByteArray());

        /// <summary>
        ///   Computes a hash from a byte buffer.
        /// </summary>
        /// <param name="buffer">The byte buffer.</param>
        /// <returns>The computed hash of the data.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is a <c>null</c> reference.</exception>
        public static ObjectId FromBytes([NotNull] byte[] buffer)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            return FromBytes(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///   Computes a hash from a byte buffer.
        /// </summary>
        /// <param name="buffer">The byte buffer.</param>
        /// <param name="offset">The offset into the <paramref name="buffer"/>.</param>
        /// <param name="count">The number of bytes to read from the <paramref name="buffer"/> starting at <paramref name="offset"/> position.</param>
        /// <returns>The computed hash of the data.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is a <c>null</c> reference.</exception>
        public static ObjectId FromBytes([NotNull] byte[] buffer, int offset, int count)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));

            var builder = new ObjectIdBuilder();
            builder.Write(buffer, offset, count);
            return builder.ComputeHash();
        }
    }
}
