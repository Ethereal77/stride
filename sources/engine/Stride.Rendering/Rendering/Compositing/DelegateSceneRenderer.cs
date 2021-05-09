// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering.Compositing
{
    /// <summary>
    /// A renderer which can provide <see cref="RendererBase.Draw"/> implementation with a delegate.
    /// </summary>
    public partial class DelegateSceneRenderer : SceneRendererBase
    {
        private readonly Action<RenderDrawContext> drawAction;

        public DelegateSceneRenderer(Action<RenderDrawContext> drawAction)
        {
            this.drawAction = drawAction;
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            drawAction(drawContext);
        }
    }
}
