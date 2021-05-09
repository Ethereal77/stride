// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Shaders;

namespace Stride.Rendering.Materials.ComputeColors
{
    /// <summary>
    /// Class ComputeVertexStreamBase.
    /// </summary>
    public abstract class ComputeVertexStreamBase : ComputeNode, IComputeVertexStream
    {
        [DataMember(10)]
        [NotNull]
        [InlineProperty]
        public IVertexStreamDefinition Stream { get; set; }

        public override ShaderSource GenerateShaderSource(ShaderGeneratorContext context, MaterialComputeColorKeys baseKeys)
        {
            var channel = GetColorChannelAsString();
            return Stream == null || string.IsNullOrWhiteSpace(Stream.GetSemanticName()) ? new ShaderClassSource("ComputeColor") : new ShaderClassSource("ComputeColorFromStream", Stream.GetSemanticName(), channel);
        }

        protected abstract string GetColorChannelAsString();
    }
}
