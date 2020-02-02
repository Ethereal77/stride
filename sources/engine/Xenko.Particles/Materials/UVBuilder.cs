// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Particles.Sorters;
using Xenko.Particles.VertexLayouts;

namespace Xenko.Particles.Materials
{
    /// <summary>
    /// Base class for building and animating the texture coordinates in a particle vertex buffer stream
    /// </summary>
    [DataContract("UVBuilder")]
    public abstract class UVBuilder
    {
        /// <summary>
        /// Enhances or animates the texture coordinates using already existing base coordinates of (0, 0, 1, 1) or similar
        /// (base texture coordinates may differ depending on the actual shape)
        /// </summary>
        /// <param name="bufferState">The particle buffer state which is used to build the assigned vertex buffer</param>
        /// <param name="sorter"><see cref="ParticleSorter"/> to use to iterate over all particles drawn this frame</param>
        /// <param name="texCoordsDescription">Attribute description of the texture coordinates in the current vertex layout</param>
        public abstract void BuildUVCoordinates(ref ParticleBufferState bufferState, ref ParticleList sorter, AttributeDescription texCoordsDescription);
    }
}
