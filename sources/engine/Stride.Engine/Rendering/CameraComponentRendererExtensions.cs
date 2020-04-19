// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Engine;
using Xenko.Rendering.Compositing;

namespace Xenko.Rendering
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
