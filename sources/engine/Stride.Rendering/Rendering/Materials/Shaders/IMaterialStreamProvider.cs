// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

namespace Xenko.Rendering.Materials
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
