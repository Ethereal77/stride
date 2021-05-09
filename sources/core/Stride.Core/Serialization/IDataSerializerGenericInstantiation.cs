// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Stride.Core.Serialization
{
    /// <summary>
    /// Allows enumeration of required data serializers.
    /// </summary>
    public interface IDataSerializerGenericInstantiation
    {
        /// <summary>
        /// Enumerates required <see cref="DataSerializer"/> required by this instance of DataSerializer.
        /// </summary>
        /// <remarks>
        /// The code won't be executed, it will only be scanned for typeof() operands by the assembly processor.
        /// Null is authorized in enumeration (for now).
        /// </remarks>
        /// <param name="serializerSelector"></param>
        /// <param name="genericInstantiations"></param>
        /// <returns></returns>
        void EnumerateGenericInstantiations(SerializerSelector serializerSelector, IList<Type> genericInstantiations);
    }
}
