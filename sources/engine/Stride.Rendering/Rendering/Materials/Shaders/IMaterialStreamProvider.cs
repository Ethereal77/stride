// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// An interface to query streams used by materials. 
    /// </summary>
    /// <remarks>
    /// This is not an exhaustive list but is used to allow to display a specific map in the editor.
    /// </remarks>
    public interface IMaterialStreamProvider
    {
        /// <summary>
        /// Gets the streams used by a material
        /// </summary>
        /// <returns>IEnumerable&lt;MaterialStream&gt;.</returns>
        IEnumerable<MaterialStreamDescriptor> GetStreams();
    }
}
