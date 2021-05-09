// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1402 // File may only contain a single class
#pragma warning disable SA1025 // Code must not contain multiple whitespace in a row

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Describes how to serialize and deserialize an object without knowing its type.
    ///   Used as a common base class for all data serializers.
    /// </summary>
    partial class DataSerializer
    {
        // Binary format version.
        // Needs to be bumped up in case of big changes in serialization format (i.e. primitive types).

        public const int BinaryFormatVersion = 4 * 1000000 // Major version: any number is ok
                                             + 0 * 10000   // Minor version: supported range: 0-99
                                             + 0 * 100     // Patch version: supported range: 0-99
                                             + 1;          // Bump ID: supported range: 0-99
    }
}
