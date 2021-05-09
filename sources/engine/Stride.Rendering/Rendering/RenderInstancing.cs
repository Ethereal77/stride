// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2020 Tebjan Halm
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering
{
    /// <summary>
    /// Contains information for model instancing. Used by the <see cref="InstancingRenderFeature"/>
    /// </summary>
    public class RenderInstancing
    {
        public int InstanceCount;
        public int ModelTransformUsage;

        // Data
        public Matrix[] WorldMatrices;
        public Matrix[] WorldInverseMatrices;

        // GPU buffers
        public bool BuffersManagedByUser;
        public Buffer InstanceWorldBuffer;
        public Buffer InstanceWorldInverseBuffer;
    }
}
