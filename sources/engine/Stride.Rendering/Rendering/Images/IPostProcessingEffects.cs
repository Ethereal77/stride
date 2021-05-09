// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Graphics;
using Stride.Rendering.Compositing;

namespace Stride.Rendering.Images
{
    public interface IPostProcessingEffects : ISharedRenderer, IDisposable
    {
        void Collect(RenderContext context);

        void Draw(RenderDrawContext drawContext, RenderOutputValidator outputValidator, Texture[] inputs, Texture inputDepthStencil, Texture outputTarget);

        bool RequiresVelocityBuffer { get; }

        bool RequiresNormalBuffer { get; }

        bool RequiresSpecularRoughnessBuffer { get; }
    }
}
