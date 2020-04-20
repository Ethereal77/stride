// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.UI.Renderers
{
    /// <summary>
    /// A factory that can create <see cref="ElementRenderer"/>s.
    /// </summary>
    public interface IElementRendererFactory
    {
        /// <summary>
        /// Try to create a renderer for the specified element.
        /// </summary>
        /// <param name="element">The element to render</param>
        /// <returns>An instance of renderer that can render it.</returns>
        ElementRenderer TryCreateRenderer(UIElement element);
    }
}
