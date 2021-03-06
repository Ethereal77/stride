// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Stride.Core.Storage;
using Stride.Graphics;
using Stride.Shaders;

namespace Stride.Rendering
{
    /// <summary>
    /// Describes an effect as used by a <see cref="RenderNode"/>.
    /// </summary>
    public class RenderEffectReflection
    {
        public static readonly RenderEffectReflection Empty = new RenderEffectReflection();

        public RootSignature RootSignature;

        public FrameResourceGroupLayout PerFrameLayout;
        public ViewResourceGroupLayout PerViewLayout;
        public RenderSystemResourceGroupLayout PerDrawLayout;

        // PerFrame
        public ResourceGroup PerFrameResources;

        public ResourceGroupBufferUploader BufferUploader;

        public EffectDescriptorSetReflection DescriptorReflection;
        public ResourceGroupDescription[] ResourceGroupDescriptions;

        // Used only for fallback effect
        public EffectParameterUpdaterLayout FallbackUpdaterLayout;
        public int[] FallbackResourceGroupMapping;
    }
}
