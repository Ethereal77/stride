// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Rendering
{
    /// <summary>
    ///   Defines the core functionality of a renderer.
    /// </summary>
    public interface IGraphicsRendererCore : IDisposable
    {
        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref="IGraphicsRenderer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this renderer is initialized.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        ///   Initializes this renderer.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        ///   This method allow a renderer to prepare for rendering.
        /// </remarks>
        void Initialize(RenderContext context);
    }
}
