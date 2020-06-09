// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Serialization
{
    /// <summary>
    ///   Defines what generic parameters to pass to a serializer.
    /// </summary>
    public enum DataSerializerGenericMode
    {
        // **************************************************************************************
        // NOTE: This file is shared with the AssemblyProcessor.
        //       If this file is modified, the AssemblyProcessor has to be recompiled separately.
        //       See build\Stride-AssemblyProcessor.sln
        // **************************************************************************************

        None = 0,

        /// <summary>
        ///   The type of the serialized type will be passed as a generic argument of the serializer.
        ///   For example, a serializer of <c>A</c> will be instantiated as a <c>Serializer{A}</c>.
        /// </summary>
        Type = 1,

        /// <summary>
        ///   The generic arguments of the serialized type will be passed as generic arguments of the serializer.
        ///   For example, a serializer of <c>A{T1, T2}</c> will be instantiated as a <c>Serializer{T1, T2}</c>.
        /// </summary>
        GenericArguments = 2,

        /// <summary>
        ///   A combination of both <see cref="Type"/> and <see cref="GenericArguments"/>.
        ///   For example, a serializer of <c>A{T1, T2}</c> will be instantiated as a <c>Serializer{A, T1, T2}</c>.
        /// </summary>
        TypeAndGenericArguments = 3
    }
}
