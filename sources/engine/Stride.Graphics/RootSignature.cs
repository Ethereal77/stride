// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Shaders;

namespace Stride.Graphics
{
    /// <summary>
    /// Describes how <see cref="DescriptorSet"/> will be bound together.
    /// </summary>
    public class RootSignature : GraphicsResourceBase
    {
        internal readonly EffectDescriptorSetReflection EffectDescriptorSetReflection;

        public static RootSignature New(GraphicsDevice graphicsDevice, EffectDescriptorSetReflection effectDescriptorSetReflection)
        {
            return new RootSignature(graphicsDevice, effectDescriptorSetReflection);
        }

        private RootSignature(GraphicsDevice graphicsDevice, EffectDescriptorSetReflection effectDescriptorSetReflection)
            : base(graphicsDevice)
        {
            this.EffectDescriptorSetReflection = effectDescriptorSetReflection;
        }

        protected internal override bool OnRecreate()
        {
            return true;
        }
    }
}
