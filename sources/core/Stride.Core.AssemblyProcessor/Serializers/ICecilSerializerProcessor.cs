// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Core.AssemblyProcessor.Serializers
{
    /// <summary>
    /// Gives the required generic serializer for a given type.
    /// This is useful for generation of serialization assembly, when AOT is performed (all generic serializers must be available).
    /// </summary>
    interface ICecilSerializerProcessor
    {
        /// <summary>
        /// Process serializers for given assembly context.
        /// </summary>
        /// <param name="context">The context.</param>
        void ProcessSerializers(CecilSerializerContext context);
    }
}
