// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.AssemblyProcessor.Serializers
{
    /// <summary>
    ///   Provides a method to retrieve the required generic serializer for a given type.
    ///   This is useful for generation of serialization assembly, when AOT is performed (all generic serializers must be available).
    /// </summary>
    interface ICecilSerializerProcessor
    {
        /// <summary>
        ///   Process serializers for a given assembly context.
        /// </summary>
        /// <param name="context">The context.</param>
        void ProcessSerializers(CecilSerializerContext context);
    }
}
