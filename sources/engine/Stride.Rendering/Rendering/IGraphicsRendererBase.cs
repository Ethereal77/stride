// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering
{
    /// <summary>
    ///   Defines the base functionality of a renderer.
    /// </summary>
    public interface IGraphicsRendererBase
    {
        /// <summary>
        ///   Draws this renderer with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        ///   The method <see cref="IGraphicsRendererCore.Initialize"/> should be called automatically by the
        ///   implementation if it was not done before the first draw.
        /// </remarks>
        void Draw(RenderDrawContext context);
    }
}
