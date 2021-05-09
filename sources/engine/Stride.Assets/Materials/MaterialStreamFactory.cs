// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Extensions;
using Stride.Rendering.Materials;

namespace Stride.Assets.Materials
{
    /// <summary>
    /// <see cref="MaterialStreamDescriptor"/> factory.
    /// </summary>
    public static class MaterialStreamFactory
    {
        /// <summary>
        /// Gets the available streams.
        /// </summary>
        /// <returns>List&lt;MaterialStreamDescriptor&gt;.</returns>
        public static List<MaterialStreamDescriptor> GetAvailableStreams()
        {
            var streams = new List<MaterialStreamDescriptor>();
            foreach (var type in typeof(IMaterialStreamProvider).GetInheritedInstantiableTypes())
            {
                if (type.GetConstructor(Type.EmptyTypes) != null)
                {
                    var provider = (IMaterialStreamProvider)Activator.CreateInstance(type);
                    streams.AddRange(provider.GetStreams());
                }
            }
            return streams;
        }
    }
}
