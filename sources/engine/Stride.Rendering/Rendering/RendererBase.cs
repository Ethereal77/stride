// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Rendering
{
    /// <summary>
    ///   Represents the base implementation of <see cref="IGraphicsRenderer"/>.
    /// </summary>
    [DataContract]
    public abstract class RendererBase : RendererCoreBase, IGraphicsRenderer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="RendererBase"/> class.
        /// </summary>
        protected RendererBase() : this(name: null) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RendererBase"/> class.
        /// </summary>
        /// <param name="name">The name attached to this renderer. Specify <c>null</c> to use the type name automatically.</param>
        protected RendererBase(string name) : base(name) { }


        /// <summary>
        ///   Main drawing method for this renderer that must be implemented.
        /// </summary>
        /// <param name="context">The remdering context.</param>
        protected abstract void DrawCore(RenderDrawContext context);

        /// <summary>
        ///   Draws this renderer with the specified context.
        /// </summary>
        /// <param name="context">The rendering context.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is a <c>null</c> reference.</exception>
        /// <exception cref="InvalidOperationException">Cannot use a different context between Load and Draw.</exception>
        public void Draw(RenderDrawContext context)
        {
            if (Enabled)
            {
                PreDrawCoreInternal(context);
                DrawCore(context);
                PostDrawCoreInternal(context);
            }
        }
    }
}
