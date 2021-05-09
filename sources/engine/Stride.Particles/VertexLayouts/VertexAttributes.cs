// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Particles.VertexLayouts
{
    /// <summary>
    /// A list of common <see cref="AttributeDescription"/> used to access the vertex fileds in a <see cref="ParticleVertexBuilder"/>
    /// </summary>
    public static class VertexAttributes
    {
        public static AttributeDescription Position = new AttributeDescription("POSITION");

        public static AttributeDescription Color = new AttributeDescription("COLOR");

        public static AttributeDescription Lifetime = new AttributeDescription("BATCH_LIFETIME");

        public static AttributeDescription RandomSeed = new AttributeDescription("BATCH_RANDOMSEED");
    }

}
