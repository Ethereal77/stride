// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Engine;
using Stride.Rendering.Compositing;

namespace Stride.Rendering
{
    public static class CameraComponentRendererExtensions
    {
        /// <summary>
        /// Property key to access the current collection of <see cref="CameraComponent"/> from <see cref="RenderContext.Tags"/>.
        /// </summary>
        public static readonly PropertyKey<CameraComponent> Current = new PropertyKey<CameraComponent>("CameraComponentRenderer.CurrentCamera", typeof(RenderContext));

        public static CameraComponent GetCurrentCamera(this RenderContext context)
        {
            return context.Tags.Get(Current);
        }
    }
}
